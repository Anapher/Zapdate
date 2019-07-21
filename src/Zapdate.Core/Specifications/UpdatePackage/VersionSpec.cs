using Zapdate.Core.Domain;

namespace Zapdate.Core.Specifications.UpdatePackage
{
    public class VersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public VersionSpec(SemVersion version)
            : base(x => x.VersionInfo.Version == version.ToString(false))
        {
        }
    }
}
