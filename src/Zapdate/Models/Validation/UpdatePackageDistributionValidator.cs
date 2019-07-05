using FluentValidation;
using Zapdate.Core.Dto.Universal;

namespace Zapdate.Models.Validation
{
    public class UpdatePackageDistributionValidator : AbstractValidator<UpdatePackageDistributionInfo>
    {
        public UpdatePackageDistributionValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
