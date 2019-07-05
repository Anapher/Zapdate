using CodeElements.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Extensions;
using Zapdate.Models.Request;

namespace Zapdate.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/projects/{projectId}/[controller]")]
    public class UpdatePackagesController : Controller
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateUpdatePackage(CreateUpdatePackageRequestDto request, int projectId, 
            [FromServices] ICreateUpdatePackageUseCase useCase)
        {
            var response = await useCase.Handle(new CreateUpdatePackageRequest(projectId, request.Version, request.Description, request.CustomFields,
                request.Files.Select(x => new Core.Dto.UseCaseRequests.UpdateFileDto(x.Path, Hash.Parse(x.Hash), x.Signature)).ToList(), request.Changelogs,
                request.Distribution));

            if (useCase.HasError)
            {
                return useCase.ToActionResult();
            }

            return Ok();
            //return CreatedAtRoute()
        }
    }
}
