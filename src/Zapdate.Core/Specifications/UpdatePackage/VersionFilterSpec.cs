using System.Collections.Generic;

namespace Zapdate.Core.Specifications.UpdatePackage
{
    public class VersionFilterSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public VersionFilterSpec(IList<string> versionFilter) : base(x =>
            x.VersionInfo.Prerelease == null || versionFilter.Contains(x.VersionInfo.Prerelease))
        {
        }
    }
}
