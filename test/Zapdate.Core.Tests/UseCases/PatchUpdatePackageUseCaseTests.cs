using CodeElements.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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
    public class PatchUpdatePackageUseCaseTests
    {
        [Fact]
        public async Task Handle_GivenInvalidUpdatePackage_ShouldFail()
        {
            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync((UpdatePackage) null);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, null);
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", "", null, new List<UpdateFileInfo>(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            ErrorUtils.AssertError(useCase, ErrorType.NotFound, ErrorCode.UpdatePackageNotFound);
        }

        [Fact]
        public async Task Handle_GivenInvalidFiles_ShouldFail()
        {
            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(new UpdatePackage("1.0.0"));

            var mockAddFiles = new Mock<IAddUpdatePackageFilesAction>();
            mockAddFiles.Setup(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            mockAddFiles.SetupGet(x => x.HasError).Returns(true);
            mockAddFiles.SetupGet(x => x.Error).Returns(new Error(ErrorType.InvalidOperation.ToString(), "Test error", -1));

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, mockAddFiles.Object);
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", "", null, new List<UpdateFileInfo> { new UpdateFileInfo("asd", Hash.Parse("FF")) },
                null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            ErrorUtils.AssertError(useCase, ErrorType.InvalidOperation);
        }

        [Fact]
        public async Task Handle_GivenValidInput_ShouldSucceed()
        {
            var updatePackage = new UpdatePackage("1.0.0");

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", "Test Description", new Dictionary<string, string> { { "shutdown", "true" } },
                new [] { new UpdateFileInfo("asd", Hash.Parse("FF")) }.ToList(),
                new [] { new UpdateChangelogInfo("de-de", "wtf") }.ToList(),
                new [] { new UpdatePackageDistributionInfo("Beta Channel", DateTimeOffset.UtcNow)}.ToList());

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal("Test Description", updatePackage.Description);
            Assert.Collection(updatePackage.CustomFields, x => Assert.Equal("shutdown", x.Key));

            var changelog = Assert.Single(updatePackage.Changelogs);
            Assert.Equal("de-de", changelog.Language);
            Assert.Equal("wtf", changelog.Content);

            var distribution = Assert.Single(updatePackage.Distributions);
            Assert.Equal("Beta Channel", distribution.Name);
            Assert.True(distribution.IsDistributing);

            mockRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_GivenNewVersion_ShouldSucceed()
        {
            var updatePackage = new UpdatePackage("1.0.0");

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var updatePackageInfo = new UpdatePackageInfo("2.0.0", null, null,
                new[] { new UpdateFileInfo("asd", Hash.Parse("FF")) }.ToList(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal("2.0.0", updatePackage.VersionInfo.Version);
            mockRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), "2.0.0", "1.0.0"), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenBuildVersion_ShouldntReorderPackages()
        {
            var updatePackage = new UpdatePackage("1.0.0-alpha");

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var updatePackageInfo = new UpdatePackageInfo("1.0.0-alpha+FFFFFF", null, null,
                new[] { new UpdateFileInfo("asd", Hash.Parse("FF")) }.ToList(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal("1.0.0-alpha+FFFFFF", updatePackage.VersionInfo.SemVersion);
            mockRepo.Verify(x => x.OrderUpdatePackages(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_GivenSameCustomFields_DontUpdateProperty()
        {
            var customFields = new Dictionary<string, string> { { "hello", "world" } };
            var customFieldsValue = customFields.ToImmutableDictionary();
            var updatePackage = new UpdatePackage("1.0.0") { CustomFields = customFieldsValue };

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, customFields, new List<UpdateFileInfo>(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal(customFieldsValue, updatePackage.CustomFields);
        }

        [Fact]
        public async Task Handle_GivenChangedChangelog_UpdateExisting()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddChangelog("de-de", "Hallo Wlt");
            updatePackage.AddChangelog("fr-fr", "Bonjour l'monde");

            var changelog = updatePackage.Changelogs.First();

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var newChangelogs = new[] { new UpdateChangelogInfo("de-de", "Hallo Welt"), new UpdateChangelogInfo("en-us", "Hello World") }.ToList();
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), newChangelogs, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Collection(updatePackage.Changelogs, x =>
            {
                Assert.Equal("de-de", x.Language);
                Assert.Equal("Hallo Welt", x.Content);
                Assert.Equal(changelog, x); // reference equal
            },
            x =>
            {
                Assert.Equal("en-us", x.Language);
                Assert.Equal("Hello World", x.Content);
            });
        }

        [Fact]
        public async Task Handle_PublishPackage_UpdateExisting()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddDistribution("enduser");

            var distribution = updatePackage.Distributions.First();

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var publishDate = DateTimeOffset.UtcNow.AddHours(-1);
            var distributions = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("enduser", publishDate) };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), null, distributions);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal(distribution, Assert.Single(updatePackage.Distributions));

            Assert.True(distribution.IsDistributing);
            Assert.Equal(publishDate, distribution.PublishDate);
        }

        [Fact]
        public async Task Handle_RollbackPackage_UpdateExisting()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddDistribution("enduser");

            var distribution = updatePackage.Distributions.First();
            distribution.Publish();

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var publishDate = DateTimeOffset.UtcNow.AddHours(-1);
            var distributions = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("enduser", DateTimeOffset.UtcNow)
                { IsRolledBack = true } };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), null, distributions);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal(distribution, Assert.Single(updatePackage.Distributions));

            Assert.False(distribution.IsDistributing);
            Assert.True(distribution.IsRolledBack);
        }

        [Fact]
        public async Task Handle_Unpublish_UpdateExisting()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddDistribution("enduser");

            var distribution = updatePackage.Distributions.First();
            distribution.Publish();

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var distributions = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("enduser", null) };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), null, distributions);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal(distribution, Assert.Single(updatePackage.Distributions));

            Assert.False(distribution.IsDistributing);
            Assert.False(distribution.IsRolledBack);
            Assert.Null(distribution.PublishDate);
        }

        [Fact]
        public async Task Handle_RepublishRolledback_UpdateExisting()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddDistribution("enduser");

            var distribution = updatePackage.Distributions.First();
            distribution.Rollback();

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var publishDate = DateTimeOffset.UtcNow.AddHours(-1);
            var distributions = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("enduser", publishDate) };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, new List<UpdateFileInfo>(), null, distributions);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            Assert.Equal(distribution, Assert.Single(updatePackage.Distributions));

            Assert.True(distribution.IsDistributing);
            Assert.False(distribution.IsRolledBack);
            Assert.Equal(publishDate, distribution.PublishDate);
        }

        [Fact]
        public async Task Handle_RenameFile_NoSigningRequired()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddFile(new UpdateFile("wrong_path.txt", "ff", "NICE SIGNATURE"));

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, GetSuccessfulAddFilesAction());
            var files = new[] { new UpdateFileInfo("right_path.txt", Hash.Parse("FF")) };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, files.ToList(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            var file = Assert.Single(updatePackage.Files);
            Assert.Equal("right_path.txt", file.Path);
            Assert.Equal("ff", file.Hash);
            Assert.Equal("NICE SIGNATURE", file.Signature);
        }

        [Fact]
        public async Task Handle_NewFiles_SigningRequired()
        {
            var updatePackage = new UpdatePackage("1.0.0");
            updatePackage.AddFile(new UpdateFile("wrong_path.txt", "ff", "NICE SIGNATURE"));

            var mockRepo = new Mock<IUpdatePackageRepository>();
            mockRepo.Setup(x => x.GetSingleBySpec(It.IsAny<ISpecification<UpdatePackage>>())).ReturnsAsync(updatePackage);

            var mockAddFiles = new Mock<IAddUpdatePackageFilesAction>();
            mockAddFiles.Setup(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            mockAddFiles.SetupGet(x => x.HasError).Returns(false);

            var useCase = new PatchUpdatePackageUseCase(mockRepo.Object, mockAddFiles.Object);
            var files = new[] { new UpdateFileInfo("path2.txt", Hash.Parse("AA")) };
            var updatePackageInfo = new UpdatePackageInfo("1.0.0", null, null, files.ToList(), null, null);

            // act
            await useCase.Handle(new PatchUpdatePackageRequest(1, "1.0.0", updatePackageInfo, null));

            // assert
            Assert.False(useCase.HasError);

            mockAddFiles.Verify(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()), Times.Once);
        }

        private IAddUpdatePackageFilesAction GetSuccessfulAddFilesAction()
        {
            var mockAddFiles = new Mock<IAddUpdatePackageFilesAction>();
            mockAddFiles.Setup(x => x.AddFiles(It.IsAny<UpdatePackage>(), It.IsAny<IEnumerable<UpdateFileInfo>>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            mockAddFiles.SetupGet(x => x.HasError).Returns(false);

            return mockAddFiles.Object;
        }
    }
}
