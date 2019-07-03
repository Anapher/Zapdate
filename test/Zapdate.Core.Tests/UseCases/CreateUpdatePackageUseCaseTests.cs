using CodeElements.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;
using Zapdate.Core.UseCases;

namespace Zapdate.Core.Tests.UseCases
{
    public class CreateUpdatePackageUseCaseTests
    {
        [Fact]
        public async Task Handle_GivenUnknownProjectId_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync((Project) null);

            var useCase = new CreateUpdatePackageUseCase(null, mockProjectRepo.Object, null, null, null);
            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, new List<UpdateFileDto>(), null, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.NotFound);
        }

        [Fact]
        public async Task Handle_GivenDuplicateChangelogLanguage_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var useCase = new CreateUpdatePackageUseCase(null, mockProjectRepo.Object, null, null, null);

            var changelogs = new List<UpdateChangelogDto>
            {
                new UpdateChangelogDto("de-de", "Hallo Welt"),
                new UpdateChangelogDto("en-us", "Hello World"),
                new UpdateChangelogDto("de-de", "Hallo Welt #2")
            };
            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, new List<UpdateFileDto>(), changelogs, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenInvalidChangelogLanguage_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var useCase = new CreateUpdatePackageUseCase(null, mockProjectRepo.Object, null, null, null);

            var changelogs = new List<UpdateChangelogDto>
            {
                new UpdateChangelogDto("de-de", "Hallo Welt"),
                new UpdateChangelogDto("en-use", "Hello World")
            };
            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, new List<UpdateFileDto>(), changelogs, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenFilesWithoutSignature_PrivateKeyNotOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, null, null, null);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF"), "")
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenFilesWithInvalidSignature_PrivateKeyNotOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockAsymCrypto = new Mock<IAsymmetricCryptoHandler>();
            mockAsymCrypto.Setup(x => x.VerifyHash(It.IsAny<Hash>(), "SIGNATURE", "PUBLIC KEY")).Returns(false);

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object, null, null);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF"), "SIGNATURE")
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenNoKeyPassword_PrivateKeyEncryptedOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "ENCRYPTED KEY", true)));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, null, null, null);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF"), "")
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenInvalidKeyPassword_PrivateKeyEncryptedOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "ENCRYPTED KEY", true)));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockSymmCrypto = new Mock<ISymmetricEncryption>();
            mockSymmCrypto.Setup(x => x.DecryptString("ENCRYPTED KEY", "Invalid Password")).Throws(new Exception("Invalid Password"));

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, null, mockSymmCrypto.Object, null);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF"), "")
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null) { AsymmetricKeyPassword = "Invalid Password" };

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_PrivateKeyPlainOnServer_ShouldSucceed()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "PLAIN KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockAsymCrypto = new Mock<IAsymmetricCryptoHandler>();
            mockAsymCrypto.Setup(x => x.SignHash(It.IsAny<Hash>(), "PLAIN KEY")).Returns("FILE SIGNATURE");

            var mockUpdatePackagesRepo = new Mock<IUpdatePackageRepository>();
            mockUpdatePackagesRepo.Setup(x => x.Add(It.IsAny<UpdatePackage>())).ReturnsAsync((UpdatePackage x) => x);

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object, null, mockUpdatePackagesRepo.Object);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF")),
                new UpdateFileDto("asd2", Hash.Parse("EE")),
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null);

            await useCase.Handle(message);

            Assert.False(useCase.HasError);
            mockUpdatePackagesRepo.Verify(x => x.Add(It.Is<UpdatePackage>(y => y.Files.Count() == 2 && y.Files.First().Signature == "FILE SIGNATURE")), Times.Once);
            mockUpdatePackagesRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "1.0.0", null), Times.Once);
        }

        [Fact]
        public async Task Handle_PrivateKeyEncryptedOnServer_ShouldSucceed()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "ENCRYPTED KEY", true)));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockSymmCrypto = new Mock<ISymmetricEncryption>();
            mockSymmCrypto.Setup(x => x.DecryptString("ENCRYPTED KEY", "Password")).Returns("PLAIN KEY");

            var mockAsymCrypto = new Mock<IAsymmetricCryptoHandler>();
            mockAsymCrypto.Setup(x => x.SignHash(It.IsAny<Hash>(), "PLAIN KEY")).Returns("FILE SIGNATURE");

            var mockUpdatePackagesRepo = new Mock<IUpdatePackageRepository>();
            mockUpdatePackagesRepo.Setup(x => x.Add(It.IsAny<UpdatePackage>())).ReturnsAsync((UpdatePackage x) => x);

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object,
                mockSymmCrypto.Object, mockUpdatePackagesRepo.Object);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF")),
                new UpdateFileDto("asd2", Hash.Parse("EE")),
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null) { AsymmetricKeyPassword = "Password" };

            await useCase.Handle(message);

            Assert.False(useCase.HasError);
            mockUpdatePackagesRepo.Verify(x => x.Add(It.Is<UpdatePackage>(y => y.Files.Count() == 2 && y.Files.First().Signature == "FILE SIGNATURE")), Times.Once);
            mockUpdatePackagesRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "1.0.0", null), Times.Once);
        }

        [Fact]
        public async Task Handle_PrivateKeyLocally_ShouldSucceed()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockAsymCrypto = new Mock<IAsymmetricCryptoHandler>();
            mockAsymCrypto.Setup(x => x.VerifyHash(It.IsAny<Hash>(), "FILE SIGNATURE", "PUBLIC KEY")).Returns(true);

            var mockUpdatePackagesRepo = new Mock<IUpdatePackageRepository>();
            mockUpdatePackagesRepo.Setup(x => x.Add(It.IsAny<UpdatePackage>())).ReturnsAsync((UpdatePackage x) => x);

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object,
                null, mockUpdatePackagesRepo.Object);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF"), "FILE SIGNATURE"),
                new UpdateFileDto("asd2", Hash.Parse("EE"), "FILE SIGNATURE"),
            };

            var message = new CreateUpdatePackageRequest(1, "1.0.0", null, null, files, null, null);

            await useCase.Handle(message);

            Assert.False(useCase.HasError);
            mockUpdatePackagesRepo.Verify(x => x.Add(It.Is<UpdatePackage>(y => y.Files.Count() == 2 && y.Files.First().Signature == "FILE SIGNATURE")), Times.Once);
            mockUpdatePackagesRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "1.0.0", null), Times.Once);
        }

        [Fact]
        public async Task Handle_NiceUpdatePackage_ShouldSucceed()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "PLAIN KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var mockAsymCrypto = new Mock<IAsymmetricCryptoHandler>();
            mockAsymCrypto.Setup(x => x.SignHash(It.IsAny<Hash>(), "PLAIN KEY")).Returns("FILE SIGNATURE");

            var mockUpdatePackagesRepo = new Mock<IUpdatePackageRepository>();
            mockUpdatePackagesRepo.Setup(x => x.Add(It.IsAny<UpdatePackage>())).ReturnsAsync((UpdatePackage x) => x);

            var useCase = new CreateUpdatePackageUseCase(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object, null, mockUpdatePackagesRepo.Object);

            var files = new List<UpdateFileDto>
            {
                new UpdateFileDto("asd", Hash.Parse("FF")),
                new UpdateFileDto("asd2", Hash.Parse("EE")),
            };

            var customFields = new Dictionary<string, string> { { "customProp", "value" } };

            var changelogs = new List<UpdateChangelogDto>
            {
                new UpdateChangelogDto("de-de", "Hallo Welt"),
                new UpdateChangelogDto("en-use", "Hello World")
            };

            var distDate = DateTimeOffset.UtcNow;
            var dist = new List<UpdatePackageDistributionDto> { new UpdatePackageDistributionDto("test", distDate) };

            var message = new CreateUpdatePackageRequest(1, "2.0.0", "Some nice update.", customFields, files, changelogs, dist);

            await useCase.Handle(message);

            Assert.False(useCase.HasError);
            mockUpdatePackagesRepo.Verify(x => x.Add(null), Times.Once);
            mockUpdatePackagesRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "1.0.0", null), Times.Once);
        }
    }
}
