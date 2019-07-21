using Zapdate.Core;
using Zapdate.Server.Core.Extensions;

namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class HasSameBinaryVersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public HasSameBinaryVersionSpec(SemVersion version)
            : base(x => x.VersionInfo.BinaryVersion == version.ToBinaryVersion())
        {
        }
    }
}
