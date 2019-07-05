using FluentValidation;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Extensions;

namespace Zapdate.Models.Validation
{
    public class UpdateChangelogValidator : AbstractValidator<UpdateChangelogDto>
    {
        public UpdateChangelogValidator()
        {
            RuleFor(x => x.Language).NotEmpty().IsValidCultureName();
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
