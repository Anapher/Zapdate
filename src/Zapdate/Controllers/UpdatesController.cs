using AutoMapper;
using CodeElements.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Core.Specifications;
using Zapdate.Extensions;
using Zapdate.Infrastructure.Data;
using Zapdate.Models.Errors;
using Zapdate.Models.Request;
using Zapdate.Models.Universal;
using Zapdate.Models.Validation;

namespace Zapdate.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/projects/{projectId}/[controller]")]
    public class UpdatesController : Controller
    {
        private readonly IMapper _mapper;

        public UpdatesController(IMapper mapper)
        {
            _mapper = mapper;
        }

        private static UpdatePackageInfo GetInfo(UpdatePackageDto dto) => new UpdatePackageInfo(dto.Version, dto.Description, dto.CustomFields,
                dto.Files.Select(x => new UpdateFileInfo(x.Path, Hash.Parse(x.Hash), x.Signature)).ToList(), dto.Changelogs,
                dto.Distribution);

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UpdatePackageDto>> CreateUpdatePackage(CreateUpdatePackageRequestDto request, int projectId,
            [FromServices] ICreateUpdatePackageUseCase useCase, [FromServices] AppDbContext context)
        {
            var updatePackage = GetInfo(request);
            var response = await useCase.Handle(new CreateUpdatePackageRequest(projectId, updatePackage, request.AsymmetricKeyPassword));

            if (useCase.HasError)
            {
                return useCase.ToActionResult();
            }

            var dto = _mapper.ProjectTo<UpdatePackageDto>(context.UpdatePackages.Where(x => x.Id == response!.UpdatePackageId)).FirstAsync();
            return CreatedAtAction(Url.Link(nameof(GetUpdatePackage), new { projectId, version = request.Version }), dto);
        }

        [HttpGet("{version}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdatePackageDto>> GetUpdatePackage(int projectId, string version, [FromServices] AppDbContext context)
        {
            if (!SemVersion.TryParse(version, out var semVersion))
            {
                return new UrlParameterValidationError("Invalid version").ToActionResult();
            }

            var query = context.UpdatePackages.Where(x => x.ProjectId == projectId && x.VersionInfo.Version == semVersion!.ToString(false));
            var package = await _mapper.ProjectTo<UpdatePackageDto>(query).FirstOrDefaultAsync();
            if (package == null)
            {
                return ResourceNotFoundError.UpdatePackageNotFound(projectId, version).ToActionResult();
            }

            return package;
        }

        [HttpPatch("{version}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PatchUpdatePackage(JsonPatchDocument<UpdatePackageDto> patchDocument, int projectId, string version,
            [FromQuery] string? password, [FromServices] IPatchUpdatePackageUseCase useCase, [FromServices] IUpdatePackageRepository repo,
            [FromServices] IMapper mapper)
        {
            // find update package
            var updatePackage = await repo.GetSingleBySpec(new FullUpdatePackageVersionSpec(version, projectId));
            if (updatePackage == null)
            {
                return ResourceNotFoundError.UpdatePackageNotFound(projectId, version).ToActionResult();
            }

            // apply patch to dto copy
            var updatePackageDto = mapper.Map<UpdatePackageDto>(updatePackage);
            patchDocument.ApplyTo(updatePackageDto);

            // validate new state
            var validator = new UpdatePackageValidator();
            var result = validator.Validate(updatePackageDto);
            if (!result.IsValid)
            {
                 return result.ToActionResult();
            }

            var updateInfo = GetInfo(updatePackageDto);
            await useCase.Handle(new PatchUpdatePackageRequest(projectId, updatePackage.VersionInfo.SemVersion, updateInfo, password));

            if (useCase.HasError)
            {
                return useCase.ToActionResult();
            }

            return Ok();
        }
    }
}
