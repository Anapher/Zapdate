using Zapdate.Core.Domain;

namespace Zapdate.Core.Specifications
{
    public class FullUpdatePackageVersionSpec : UpdatePackageVersionSpec
    {
        public FullUpdatePackageVersionSpec(SemVersion version, int projectId) : base(version, projectId)
        {
            AddInclude(x => x.Changelogs);
            AddInclude(x => x.Files);
            AddInclude(x => x.Distributions);
        }
    }
}
