using FluentValidation;
using Zapdate.Server.Models.Request;

namespace Zapdate.Server.Models.Validation
{
    public class CreateUpdatePackageRequestValidator : AbstractValidator<CreateUpdatePackageRequestDto>
    {
        public CreateUpdatePackageRequestValidator()
        {
            RuleFor(x => x).SetValidator(new UpdatePackageValidator());
        }
    }
}
