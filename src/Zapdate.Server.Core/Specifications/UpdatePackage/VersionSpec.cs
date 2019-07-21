using Zapdate.Core;

namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class VersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public VersionSpec(SemVersion version)
            : base(x => x.VersionInfo.Version == version.ToString(false))
        {
        }
    }
}
