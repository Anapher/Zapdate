#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

using System.Collections.Generic;
using Zapdate.Core.Dto.UseCaseRequests;

namespace Zapdate.Models.Request
{
    public class CreateUpdatePackageRequestDto
    {
        public string Version { get; set; }
        public string? Description { get; set; }
        public IDictionary<string, string>? CustomFields { get; set; }

        public IList<UpdateFileDto> Files { get; set; }
        public IList<UpdateChangelogDto>? Changelogs { get; set; }
        public IList<UpdatePackageDistributionDto>? Distribution { get; set; }
    }
}
