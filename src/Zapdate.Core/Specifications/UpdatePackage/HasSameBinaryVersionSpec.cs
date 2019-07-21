using Zapdate.Core.Domain;
using Zapdate.Core.Extensions;

namespace Zapdate.Core.Specifications.UpdatePackage
{
    public class HasSameBinaryVersionSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public HasSameBinaryVersionSpec(SemVersion version)
            : base(x => x.VersionInfo.BinaryVersion == version.ToBinaryVersion())
        {
        }
    }
}
