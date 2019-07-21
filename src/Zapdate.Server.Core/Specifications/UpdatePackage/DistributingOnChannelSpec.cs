using System.Linq;

namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class DistributingOnChannelSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public DistributingOnChannelSpec(string channel) : base(updatePackage =>
            updatePackage.IsListed && updatePackage.Distributions.Any(x => x.Name == channel && x.IsDistributing))
        {
        }
    }
}
