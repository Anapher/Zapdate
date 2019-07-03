using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Core.Specifications
{
    public class UpdatePackageVersionSpec : BaseSpecification<UpdatePackage>
    {
        public UpdatePackageVersionSpec(SemVersion version) : base(x => x.VersionInfo.Version == version.ToString(false))
        {
        }
    }
}
