using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Interfaces.Gateways.Repositories;

namespace Zapdate.Infrastructure.Data.Repositories
{
    public class ProjectRepository : EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
