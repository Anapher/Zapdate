using Zapdate.Core;
using Zapdate.Server.Core.Extensions;

namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class HasLowerBinaryVersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public HasLowerBinaryVersionSpec(SemVersion version)
            : base(x => x.VersionInfo.BinaryVersion < version.ToBinaryVersion())
        {
        }
    }
}
