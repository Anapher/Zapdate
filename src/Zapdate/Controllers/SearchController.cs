using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Extensions;
using Zapdate.Models.Errors;

namespace Zapdate.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/v1/projects/{projectId}/[controller]")]
    public class SearchController : Controller
    {
        // GET api/v1/projects/{projectId}/search/{channel}?version&versionFilter
        [HttpGet("{channel}")]
        public async Task<ActionResult> SearchForUpdates(int projectId, string channel, [FromQuery] string version,
            [FromQuery] string? versionFilter, [FromServices] ISearchUpdateUseCase searchUseCase)
        {
            if (!SemVersion.TryParse(version, out var semVersion))
                return new UrlParameterValidationError("Invalid version").ToActionResult();

            var versionFilterList = string.IsNullOrEmpty(versionFilter) ? new List<string>() : versionFilter!.Split(',').ToList();

            var response = await searchUseCase.Handle(new SearchUpdateRequest(projectId, semVersion!, channel, versionFilterList));
            if (searchUseCase.HasError)
                return searchUseCase.Error!.ToActionResult();

            if (response!.RecommendedPackage == null)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
