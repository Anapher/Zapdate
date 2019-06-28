using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Infrastructure.Data;
using Zapdate.Models.Response;
using Zapdate.Models.ObjectQueries;
using Microsoft.EntityFrameworkCore;
using System;
using Zapdate.Models.Request;

namespace Zapdate.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ProjectsController : Controller
    {
        // GET api/v1/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects([FromServices] AppDbContext context)
        {
            return await context.Projects.MapProjectsToDto().ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject(CreateProjectRequestDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
