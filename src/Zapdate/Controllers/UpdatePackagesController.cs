using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Zapdate.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/projects/{projectId}/[controller]")]
    public class UpdatePackagesController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CreateUpdatePackage()
        {
            throw new NotImplementedException();
        }
    }
}
