using FluentValidation;
using Zapdate.Core.Dto.Universal;
using Zapdate.Extensions;

namespace Zapdate.Models.Validation
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
