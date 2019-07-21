using FluentValidation;
using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.Extensions;

namespace Zapdate.Server.Models.Validation
{
    public class UpdateChangelogValidator : AbstractValidator<UpdateChangelogInfo>
    {
        public UpdateChangelogValidator()
        {
            RuleFor(x => x.Language).NotEmpty().IsValidCultureName();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
