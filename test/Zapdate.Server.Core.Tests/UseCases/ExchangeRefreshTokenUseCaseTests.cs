using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Core.UseCases;
using Moq;
using System.Security.Claims;
using Xunit;
using System.Threading.Tasks;

namespace Zapdate.Server.Core.Tests.UseCases
{
    public class ExchangeRefreshTokenUseCaseUnitTests
    {
        [Fact]
        public async Task Handle_GivenInvalidToken_ShouldFail()
        {
            // arrange
            var mockJwtTokenValidator = new Mock<IJwtValidator>();
            mockJwtTokenValidator.Setup(validator => validator.GetPrincipalFromToken(It.IsAny<string>())).Returns((ClaimsPrincipal)null);

            var useCase = new ExchangeRefreshTokenUseCase(mockJwtTokenValidator.Object, null, null, null);

            // act
            var response = await useCase.Handle(new ExchangeRefreshTokenRequest("", "", ""));

            // assert
            Assert.True(useCase.HasError);
        }

        [Fact]
        public async Task Handle_GivenValidToken_ShouldSucceed()
        {
            // arrange
            var mockJwtTokenValidator = new Mock<IJwtValidator>();
            mockJwtTokenValidator.Setup(validator => validator.GetPrincipalFromToken(It.IsAny<string>())).Returns(new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new []{ new Claim("id","111-222-333")})
            }));

            const string refreshToken = "1234";
            var user = new User("", "", "");
            user.AddRefreshToken(refreshToken, "");

            var mockUserRepository = new Mock<IUserRepository>();
            mockUserRepository.Setup(repo => repo.FindById(It.IsAny<string>())).ReturnsAsync(user);

            var mockJwtFactory = new Mock<IJwtFactory>();
            mockJwtFactory.Setup(factory => factory.GenerateEncodedToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("");

            var mockTokenFactory = new Mock<ITokenFactory>();
            mockTokenFactory.Setup(factory => factory.GenerateToken(32)).Returns("");

            var useCase = new ExchangeRefreshTokenUseCase(mockJwtTokenValidator.Object, mockUserRepository.Object, mockJwtFactory.Object, mockTokenFactory.Object);

            // act
            var response = await useCase.Handle(new ExchangeRefreshTokenRequest("", refreshToken, ""));

            // assert
            Assert.False(useCase.HasError);
            Assert.NotNull(response);
        }
    }
}
