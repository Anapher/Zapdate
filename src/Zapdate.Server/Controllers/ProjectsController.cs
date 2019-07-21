using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Server.Infrastructure.Data;
using Zapdate.Server.Models.Response;
using Zapdate.Server.Models.ObjectQueries;
using Microsoft.EntityFrameworkCore;
using Zapdate.Server.Models.Request;
using Zapdate.Server.Core.Interfaces.UseCases;
using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Extensions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Zapdate.Server.Controllers
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

        // api/v1/projects/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto?>> GetProject(int id, [FromServices] AppDbContext context)
        {
            return await context.Projects.Where(x => x.Id == id).MapProjectsToDto().FirstOrDefaultAsync();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateProjectResponseDto>> CreateProject(CreateProjectRequestDto dto, [FromServices] ICreateProjectUseCase useCase)
        {
            var result = await useCase.Handle(new CreateProjectRequest(dto.Name, dto.RsaKeyStorage, dto.RsaKeyPassword));
            if (useCase.HasError)
            {
                return useCase.ToActionResult();
            }

            return new CreateProjectResponseDto(result!.ProjectId, result.AsymmetricKey);
        }
    }
}
