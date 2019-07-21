namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class IncludeDistributionsSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public IncludeDistributionsSpec() : base(_ => true)
        {
            AddInclude(x => x.Distributions);
        }
    }
}
