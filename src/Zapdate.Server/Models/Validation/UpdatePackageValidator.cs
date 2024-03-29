using FluentValidation;
using Zapdate.Server.Extensions;
using Zapdate.Server.Models.Universal;

namespace Zapdate.Server.Models.Validation
{
    public class UpdatePackageValidator : AbstractValidator<UpdatePackageDto>
    {
        public UpdatePackageValidator()
        {
            RuleFor(x => x.Version).NotEmpty().IsSemanticVersion();
            RuleFor(x => x.Description).MaximumLength(512);
            RuleFor(x => x.Files).NotEmpty().ForEach(x => x.SetValidator(new UpdateFileDtoValidator()))
                .IsUniqueList(x => x.Path, "The files must have a unique path.");
            RuleFor(x => x.Changelogs).ForEach(x => x.SetValidator(new UpdateChangelogValidator()))
                .IsUniqueList(x => x.Language, "The changelogs must have unique languages.");
            RuleFor(x => x.Distribution).ForEach(x => x.SetValidator(new UpdatePackageDistributionValidator()))
                .IsUniqueList(x => x.Name);
        }
    }
}
