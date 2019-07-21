namespace Zapdate.Server.Core.Specifications.UpdatePackage
{
    public class ProjectSpec : BaseSpecification<Domain.Entities.UpdatePackage>
    {
        public ProjectSpec(int projectId) : base(x => x.ProjectId == projectId)
        {
        }
    }
}
