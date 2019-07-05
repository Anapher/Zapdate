using FluentValidation;
using Zapdate.Core.Dto.UseCaseRequests;

namespace Zapdate.Models.Validation
{
    public class UpdatePackageDistributionValidator : AbstractValidator<UpdatePackageDistributionDto>
    {
        public UpdatePackageDistributionValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
