namespace Zapdate.Core.Specifications.UpdatePackage
{
    public class IncludeAllSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public IncludeAllSpec() : base(_ => true)
        {
            AddInclude(x => x.Changelogs);
            AddInclude(x => x.Files);
            AddInclude(x => x.Distributions);
        }
    }
}
