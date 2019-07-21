using Zapdate.Core.Domain;
using Zapdate.Core.Extensions;

namespace Zapdate.Core.Specifications.UpdatePackage
{
    public class HasLowerBinaryVersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public HasLowerBinaryVersionSpec(SemVersion version)
            : base(x => x.VersionInfo.BinaryVersion < version.ToBinaryVersion())
        {
        }
    }
}
