using FluentValidation;
using Zapdate.Models.Request;

namespace Zapdate.Models.Validation
{
    public class CreateUpdatePackageRequestValidator : AbstractValidator<CreateUpdatePackageRequestDto>
    {
        public CreateUpdatePackageRequestValidator()
        {
            RuleFor(x => x).SetValidator(new UpdatePackageValidator());
        }
    }
}
