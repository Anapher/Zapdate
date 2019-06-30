using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Infrastructure.Data;
using Zapdate.Models.Response;
using Zapdate.Models.ObjectQueries;
using Microsoft.EntityFrameworkCore;
using Zapdate.Models.Request;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Extensions;
using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CreateProjectResponseDto>> CreateProject(CreateProjectRequestDto dto, [FromServices] ICreateProjectUseCase useCase)
        {
            var result = await useCase.Handle(new CreateProjectRequest(dto.ProjectName, dto.RsaKeyStorage, dto.RsaKeyPassword));
            if (useCase.HasError)
            {
                return useCase.ToActionResult();
            }

            return new CreateProjectResponseDto(result!.ProjectId, result.AsymmetricKey);
        }
    }
}
