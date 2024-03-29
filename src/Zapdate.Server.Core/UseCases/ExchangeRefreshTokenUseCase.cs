using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Errors;
using Zapdate.Server.Core.Interfaces;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Core.Interfaces.UseCases;
using System.Linq;
using System.Threading.Tasks;

namespace Zapdate.Server.Core.UseCases
{
    public class ExchangeRefreshTokenUseCase : UseCaseStatus<ExchangeRefreshTokenResponse>, IExchangeRefreshTokenUseCase
    {
        private readonly IJwtValidator _jwtValidator;
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        public ExchangeRefreshTokenUseCase(IJwtValidator jwtValidator, IUserRepository userRepository, IJwtFactory jwtFactory, ITokenFactory tokenFactory)
        {
            _jwtValidator = jwtValidator;
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
        }

        public async Task<ExchangeRefreshTokenResponse?> Handle(ExchangeRefreshTokenRequest message)
        {
            var claimsPrincipal = _jwtValidator.GetPrincipalFromToken(message.AccessToken);
            if (claimsPrincipal == null)
            {
                // invalid token/signing key was passed and we can't extract user claims
                return ReturnError(AuthenticationError.InvalidToken);
            }

            var id = claimsPrincipal.Claims.First(x => x.Type == "id");
            var user = await _userRepository.FindById(id.Value);
            if (user == null)
                return ReturnError(AuthenticationError.UserNotFound);

            if (!user.HasValidRefreshToken(message.RefreshToken))
                return ReturnError(AuthenticationError.InvalidToken);

            var jwToken = await _jwtFactory.GenerateEncodedToken(user.Id, user.UserName);
            var refreshToken = _tokenFactory.GenerateToken();

            user.RemoveRefreshToken(message.RefreshToken);
            user.AddRefreshToken(refreshToken, message.RemoteIpAddress);

            await _userRepository.Update(user);
            return new ExchangeRefreshTokenResponse(jwToken, refreshToken);
        }
    }
}
