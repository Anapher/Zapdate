using FluentValidation;
using Zapdate.Core.Domain;
using Zapdate.Models.Request;

namespace Zapdate.Models.Validation
{
    public class CreateUpdatePackageRequestValidator : AbstractValidator<CreateUpdatePackageRequestDto>
    {
        public CreateUpdatePackageRequestValidator()
        {
            RuleFor(x => x.Version).NotEmpty().Must(x => SemVersion.TryParse(x, out _));
            RuleFor(x => x.Description).MaximumLength(512);
            RuleFor(x => x.Files).NotEmpty();
        }
    }
}
