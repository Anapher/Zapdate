using CodeElements.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Domain.Actions;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;

namespace Zapdate.Core.Tests.Domain.Actions
{
    public class AddUpdatePackageFilesActionTests
    {
        [Fact]
        public async Task Handle_GivenFilesWithoutSignature_PrivateKeyNotOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, null, null);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"), "")
            };
            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, null);

            // assert
            ErrorUtils.AssertError(action, ErrorType.ValidationError);
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

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object, null);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"), "SIGNATURE")
            };

            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, null);

            // assert
            ErrorUtils.AssertError(action, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenNoKeyPassword_PrivateKeyEncryptedOnServer_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "ENCRYPTED KEY", true)));

            var mockFilesRepo = new Mock<IStoredFileRepository>();
            mockFilesRepo.Setup(x => x.FindByHash(It.IsAny<Hash>())).ReturnsAsync(new StoredFile("HASH", 100, 80));

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, null, null);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"), "")
            };
            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, null);

            // assert
            ErrorUtils.AssertError(action, ErrorType.ValidationError);
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

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, null, mockSymmCrypto.Object);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"), "")
            };

            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, "Invalid Password");

            // assert
            ErrorUtils.AssertError(action, ErrorType.ValidationError);
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

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object, null);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF")),
                new UpdateFileInfo("asd2", Hash.Parse("EE")),
            };

            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, null);

            // assert
            Assert.False(action.HasError);

            Assert.Equal(2, package.Files.Count());
            Assert.Collection(package.Files, x => Assert.Equal("FILE SIGNATURE", x.Signature), x => Assert.Equal("FILE SIGNATURE", x.Signature));
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

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object,
                mockSymmCrypto.Object);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF")),
                new UpdateFileInfo("asd2", Hash.Parse("EE")),
            };

            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, "Password");

            // assert
            Assert.False(action.HasError);

            Assert.Equal(2, package.Files.Count());
            Assert.Collection(package.Files, x => Assert.Equal("FILE SIGNATURE", x.Signature), x => Assert.Equal("FILE SIGNATURE", x.Signature));
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

            var action = new AddUpdatePackageFilesAction(mockFilesRepo.Object, mockProjectRepo.Object, mockAsymCrypto.Object,
                null);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"), "FILE SIGNATURE"),
                new UpdateFileInfo("asd2", Hash.Parse("EE"), "FILE SIGNATURE"),
            };

            var package = new UpdatePackage("1.0.0");

            // act
            await action.AddFiles(package, files, null);

            // assert
            Assert.False(action.HasError);

            Assert.Equal(2, package.Files.Count());
            Assert.Collection(package.Files, x => Assert.Equal("FILE SIGNATURE", x.Signature), x => Assert.Equal("FILE SIGNATURE", x.Signature));
        }
    }
}
