using System.Linq;
using Zapdate.Core.Domain.Entities;
using Zapdate.Models.Response;

namespace Zapdate.Models.ObjectQueries
{
    public static class ProjectsQueriesExtensions
    {
        public static IQueryable<ProjectDto> MapProjectsToDto(this IQueryable<Project> projects)
        {
            return projects.Select(x => new ProjectDto(x.Id, x.Name));
        }
    }
}
