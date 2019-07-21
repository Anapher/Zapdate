using Moq;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Dto.Services;
using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Core.UseCases;

namespace Zapdate.Server.Core.Tests.UseCases
{
    public class CreateProjectUseCaseTests
    {
        [Fact]
        public async Task Handle_GivenNoProjectName_ShouldFail()
        {
            var useCase = new CreateProjectUseCase(null, null, null);
            var response = await useCase.Handle(new CreateProjectRequest("", KeyStorage.Server, null));

            Assert.Null(response);
            Assert.True(useCase.HasError);
        }

        [Fact]
        public async Task Handle_GivenNoKeyIfEncrypted_ShouldFail()
        {
            var useCase = new CreateProjectUseCase(null, null, null);
            var response = await useCase.Handle(new CreateProjectRequest("Name", KeyStorage.ServerEncrypted, null));

            Assert.Null(response);
            Assert.True(useCase.HasError);
        }

        [Fact]
        public async Task Handle_ServerStorage_ShouldSucceed()
        {
            var mockAsymmetricKeyParametersFactory = new Mock<IAsymmetricKeyParametersFactory>();
            mockAsymmetricKeyParametersFactory.Setup(x => x.Create()).Returns(new AsymmetricKeyParameters("PUBLIC KEY", "PRIVATE KEY"));

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(x => x.Add(It.IsAny<Project>())).ReturnsAsync((Project x) => x);

            var useCase = new CreateProjectUseCase(mockAsymmetricKeyParametersFactory.Object, mockRepository.Object, null);
            var response = await useCase.Handle(new CreateProjectRequest("Name", KeyStorage.Server, null));
            Assert.False(useCase.HasError);

            Assert.Null(response.AsymmetricKey);

            mockRepository.Verify(repo => repo.Add(It.Is<Project>(x => x.Name == "Name" && !x.AsymmetricKey.IsPrivateKeyEncrypted && x.AsymmetricKey.PrivateKey == "PRIVATE KEY" && x.AsymmetricKey.PublicKey == "PUBLIC KEY")), Times.Once);
        }

        [Fact]
        public async Task Handle_ServerEncryptedStorage_ShouldSucceed()
        {
            var mockAsymmetricKeyParametersFactory = new Mock<IAsymmetricKeyParametersFactory>();
            mockAsymmetricKeyParametersFactory.Setup(x => x.Create()).Returns(new AsymmetricKeyParameters("PUBLIC KEY", "PRIVATE KEY"));

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(x => x.Add(It.IsAny<Project>())).ReturnsAsync((Project x) => x);

            var mockSymmetricEncryption = new Mock<ISymmetricEncryption>();
            mockSymmetricEncryption.Setup(x => x.EncryptString("PRIVATE KEY", "My Password")).Returns("ENCRYPTED PRIVATE KEY");

            var useCase = new CreateProjectUseCase(mockAsymmetricKeyParametersFactory.Object, mockRepository.Object, mockSymmetricEncryption.Object);
            var response = await useCase.Handle(new CreateProjectRequest("Name", KeyStorage.ServerEncrypted, "My Password"));
            Assert.False(useCase.HasError);

            Assert.Null(response.AsymmetricKey);

            mockRepository.Verify(repo => repo.Add(It.Is<Project>(x => x.Name == "Name" && x.AsymmetricKey.IsPrivateKeyEncrypted && x.AsymmetricKey.PrivateKey == "ENCRYPTED PRIVATE KEY" && x.AsymmetricKey.PublicKey == "PUBLIC KEY")), Times.Once);
        }

        [Fact]
        public async Task Handle_LocallyStorage_ShouldSucceed()
        {
            var mockAsymmetricKeyParametersFactory = new Mock<IAsymmetricKeyParametersFactory>();
            mockAsymmetricKeyParametersFactory.Setup(x => x.Create()).Returns(new AsymmetricKeyParameters("PUBLIC KEY", "PRIVATE KEY"));

            var mockRepository = new Mock<IProjectRepository>();
            mockRepository.Setup(x => x.Add(It.IsAny<Project>())).ReturnsAsync((Project x) => x);

            var useCase = new CreateProjectUseCase(mockAsymmetricKeyParametersFactory.Object, mockRepository.Object, null);
            var response = await useCase.Handle(new CreateProjectRequest("Name", KeyStorage.Locally, null));
            Assert.False(useCase.HasError);

            Assert.Equal("PRIVATE KEY", response.AsymmetricKey);

            mockRepository.Verify(repo => repo.Add(It.Is<Project>(x => x.Name == "Name" && !x.AsymmetricKey.IsPrivateKeyEncrypted
                && x.AsymmetricKey.PrivateKey == null && x.AsymmetricKey.PublicKey == "PUBLIC KEY")), Times.Once);
        }
    }
}