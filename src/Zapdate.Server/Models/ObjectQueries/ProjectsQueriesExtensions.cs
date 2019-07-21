using System.Linq;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Models.Response;

namespace Zapdate.Server.Models.ObjectQueries
{
    public static class ProjectsQueriesExtensions
    {
        public static IQueryable<ProjectDto> MapProjectsToDto(this IQueryable<Project> projects)
        {
            return projects.Select(x => new ProjectDto(x.Id, x.Name, x.DistributionChannels));
        }
    }
}
