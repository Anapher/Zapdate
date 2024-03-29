using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Core.UseCases;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace Zapdate.Server.Core.Tests.UseCases
{
    public class LoginUseCaseTests
    {
        [Fact]
        public async Task Handle_GivenValidCredentials_ShouldSucceed()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", ""));

            mockUserRepository.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUseCase(mockUserRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // act
            var response = await useCase.Handle(new LoginRequest("userName", "password", "127.0.0.1"));

            // assert
            Assert.False(useCase.HasError);
        }

        [Fact]
        public async Task Handle_GivenIncompleteCredentials_ShouldFail()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync(new User("", "", ""));

            mockUserRepository.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUseCase(mockUserRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // act
            await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"));

            // assert
            Assert.True(useCase.HasError);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);
        }

        [Fact]
        public async Task Handle_GivenUnknownCredentials_ShouldFail()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync((User)null);

            mockUserRepository.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUseCase(mockUserRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // act
            await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"));

            // assert
            Assert.True(useCase.HasError);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);
        }

        [Fact]
        public async Task Handle_GivenInvalidPassword_ShouldFail()
        {
            // arrange
            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindByName(It.IsAny<string>())).ReturnsAsync((User)null);

            mockUserRepository.Setup(repo => repo.CheckPassword(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");

            var mockTokenFactory = new Mock<ITokenFactory>();

            var useCase = new LoginUseCase(mockUserRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // act
            await useCase.Handle(new LoginRequest("", "password", "127.0.0.1"));

            // assert
            Assert.True(useCase.HasError);
            mockTokenFactory.Verify(factory => factory.GenerateToken(32), Times.Never);
        }
    }
}
