using Zapdate.Core.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Domain.Actions;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces.Gateways.Repositories;
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

            var useCase = new CreateUpdatePackageUseCase(mockProjectRepo.Object, null, null);

            var package = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), null, null);
            var message = new CreateUpdatePackageRequest(1, package);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.NotFound);
        }

        [Fact]
        public async Task Handle_GivenDuplicateChangelogLanguage_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var useCase = new CreateUpdatePackageUseCase(mockProjectRepo.Object, null, null);

            var changelogs = new List<UpdateChangelogInfo>
            {
                new UpdateChangelogInfo("de-de", "Hallo Welt"),
                new UpdateChangelogInfo("en-us", "Hello World"),
                new UpdateChangelogInfo("de-de", "Hallo Welt #2")
            };
            var package = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), changelogs, null);
            var message = new CreateUpdatePackageRequest(1, package);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_GivenInvalidChangelogLanguage_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var useCase = new CreateUpdatePackageUseCase(mockProjectRepo.Object, null, null);

            var changelogs = new List<UpdateChangelogInfo>
            {
                new UpdateChangelogInfo("de-de", "Hallo Welt"),
                new UpdateChangelogInfo("en-use", "Hello World")
            };

            var package = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), changelogs, null);
            var message = new CreateUpdatePackageRequest(1, package);

            await useCase.Handle(message);

            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_AddFilesFail_ShouldFail()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY")));

            var mockAddFiles = new Mock<IAddUpdatePackageFilesAction>();
            mockAddFiles.Setup(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            mockAddFiles.SetupGet(x => x.HasError).Returns(true);
            mockAddFiles.SetupGet(x => x.Error).Returns(new Error(ErrorType.ValidationError.ToString(), "Test error", -1));

            var useCase = new CreateUpdatePackageUseCase(mockProjectRepo.Object, null, mockAddFiles.Object);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF"))
            };
            var package = new UpdatePackageInfo("1.0.0", null, null, files, null, null);
            var message = new CreateUpdatePackageRequest(1, package);

            // act
            await useCase.Handle(message);

             // assert
            ErrorUtils.AssertError(useCase, ErrorType.ValidationError);
        }

        [Fact]
        public async Task Handle_NiceUpdatePackage_ShouldSucceed()
        {
            var mockProjectRepo = new Mock<IProjectRepository>();
            mockProjectRepo.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new Project("test", new AsymmetricKey("PUBLIC KEY", "PLAIN KEY")));

            var mockUpdatePackagesRepo = new Mock<IUpdatePackageRepository>();
            UpdatePackage savedUpdatePackage = null;
            mockUpdatePackagesRepo.Setup(x => x.Add(It.IsAny<UpdatePackage>())).Callback<UpdatePackage>(x => savedUpdatePackage = x)
                .ReturnsAsync((UpdatePackage x) => x);

            var mockAddFiles = new Mock<IAddUpdatePackageFilesAction>();
            mockAddFiles.Setup(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()))
                .Callback((UpdatePackage x, IEnumerable<UpdateFileInfo> y, string _) =>
                {
                    foreach (var file in y)
                        x.AddFile(new UpdateFile(file.Path, file.Hash.ToString(), "FILE SIGNATURE"));
                }).Returns(Task.CompletedTask);

            var useCase = new CreateUpdatePackageUseCase(mockProjectRepo.Object, mockUpdatePackagesRepo.Object, mockAddFiles.Object);

            var files = new List<UpdateFileInfo>
            {
                new UpdateFileInfo("asd", Hash.Parse("FF")),
                new UpdateFileInfo("asd2", Hash.Parse("EE")),
            };

            var customFields = new Dictionary<string, string> { { "customProp", "value" } };

            var changelogs = new List<UpdateChangelogInfo>
            {
                new UpdateChangelogInfo("de-de", "Hallo Welt"),
                new UpdateChangelogInfo("en-us", "Hello World")
            };

            var distDate = DateTimeOffset.UtcNow;
            var dist = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("test", distDate) };

            var package = new UpdatePackageInfo("2.0.0", "Some nice update.", customFields, files, changelogs, dist);

            var message = new CreateUpdatePackageRequest(1, package, null);

            await useCase.Handle(message);

            Assert.False(useCase.HasError);
            mockUpdatePackagesRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "2.0.0", null), Times.Once);

            Assert.NotNull(savedUpdatePackage);

            Assert.Equal(message.UpdatePackage.Description, savedUpdatePackage.Description);
            Assert.Equal(message.UpdatePackage.Version, savedUpdatePackage.VersionInfo.SemVersion);
            Assert.Equal(message.UpdatePackage.CustomFields, savedUpdatePackage.CustomFields);
            Assert.Collection(savedUpdatePackage.Changelogs, x => Assert.Equal("de-de", x.Language), x => Assert.Equal("en-us", x.Language));
            Assert.Collection(savedUpdatePackage.Distributions, x => 
            {
                Assert.Equal("test", x.Name);
                Assert.True(x.IsPublished);
                Assert.Equal(distDate, x.PublishDate);
            });
            Assert.Collection(savedUpdatePackage.Files, x => Assert.Equal("asd", x.Path), x => Assert.Equal("asd2", x.Path));
        }
    }
}
