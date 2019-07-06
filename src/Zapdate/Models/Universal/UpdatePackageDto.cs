#pragma warning disable CS8618 // Non-nullable field is uninitialized. Validators gurantee that.

using System.Collections.Generic;
using Zapdate.Core.Dto.Universal;

namespace Zapdate.Models.Universal
{
    public class UpdatePackageDto
    {
        public string Version { get; set; }
        public string? Description { get; set; }
        public IDictionary<string, string>? CustomFields { get; set; }

        public IList<UpdateFileDto> Files { get; set; }
        public IList<UpdateChangelogInfo>? Changelogs { get; set; }
        public IList<UpdatePackageDistributionInfo>? Distribution { get; set; }
    }
}
