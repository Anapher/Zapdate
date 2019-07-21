using FluentValidation;
using Zapdate.Server.Core.Dto.Universal;

namespace Zapdate.Server.Models.Validation
{
    public class UpdatePackageDistributionValidator : AbstractValidator<UpdatePackageDistributionInfo>
    {
        public UpdatePackageDistributionValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
