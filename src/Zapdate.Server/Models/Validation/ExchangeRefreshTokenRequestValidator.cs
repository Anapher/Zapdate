using Zapdate.Server.Models.Request;
using FluentValidation;

namespace Zapdate.Server.Models.Validation
{
    public class ExchangeRefreshTokenRequestValidator : AbstractValidator<ExchangeRefreshTokenRequestDto>
    {
        public ExchangeRefreshTokenRequestValidator()
        {
            RuleFor(x => x.AccessToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
