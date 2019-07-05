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

        public IReadOnlyList<UpdateFileDto> Files { get; set; }
        public IReadOnlyList<UpdateChangelogDto>? Changelogs { get; set; }
        public IReadOnlyList<UpdatePackageDistributionDto>? Distribution { get; set; }
    }

    public class UpdateFileDto
    {
        public string Path { get; set; }
        public string Hash { get; set; }
        public string? Signature { get; set; }
    }
}
