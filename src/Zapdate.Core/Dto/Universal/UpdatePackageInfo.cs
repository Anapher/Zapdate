using System.Collections.Generic;
using Zapdate.Core.Domain;

namespace Zapdate.Core.Dto.Universal
{
    public class UpdatePackageInfo
    {
        public UpdatePackageInfo(SemVersion version, string? description, IDictionary<string, string>? customFields,
            IReadOnlyList<UpdateFileInfo> files, IReadOnlyList<UpdateChangelogInfo>? changelogs,
            IReadOnlyList<UpdatePackageDistributionInfo>? distribution)
        {
            Version = version;
            Description = description;
            CustomFields = customFields ?? new Dictionary<string, string>();
            Files = files;
            Changelogs = changelogs;
            Distributions = distribution;
        }

        public SemVersion Version { get; }
        public string? Description { get; }
        public IDictionary<string, string> CustomFields { get; }

        public IReadOnlyList<UpdateFileInfo> Files { get; set; }
        public IReadOnlyList<UpdateChangelogInfo>? Changelogs { get; set; }
        public IReadOnlyList<UpdatePackageDistributionInfo>? Distributions { get; set; }
    }
}
