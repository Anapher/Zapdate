using Zapdate.Models.Request;
using FluentValidation;

namespace Zapdate.Models.Validation
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
