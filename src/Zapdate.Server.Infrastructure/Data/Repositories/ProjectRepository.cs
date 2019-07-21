using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;

namespace Zapdate.Server.Infrastructure.Data.Repositories
{
    public class ProjectRepository : EfRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
