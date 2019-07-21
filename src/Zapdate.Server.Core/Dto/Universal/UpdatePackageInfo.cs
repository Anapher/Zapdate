using System.Collections.Generic;
using System.Collections.Immutable;
using Zapdate.Core;

namespace Zapdate.Server.Core.Dto.Universal
{
    public class UpdatePackageInfo
    {
        public UpdatePackageInfo(SemVersion version, string? description, IImmutableDictionary<string, string>? customFields,
            IImmutableList<UpdateFileInfo> files, IImmutableList<UpdateChangelogInfo>? changelogs,
            IImmutableList<UpdatePackageDistributionInfo>? distribution)
        {
            Version = version;
            Description = description;
            CustomFields = customFields ?? ImmutableDictionary<string, string>.Empty;
            Files = files;
            Changelogs = changelogs ?? ImmutableList<UpdateChangelogInfo>.Empty;
            Distributions = distribution ?? ImmutableList<UpdatePackageDistributionInfo>.Empty;
        }

        public UpdatePackageInfo(SemVersion version, string? description, IDictionary<string, string>? customFields,
            IEnumerable<UpdateFileInfo> files, IEnumerable<UpdateChangelogInfo>? changelogs,
            IEnumerable<UpdatePackageDistributionInfo>? distribution)
            : this(version, description, customFields?.ToImmutableDictionary(), files.ToImmutableList(), changelogs?.ToImmutableList(),
                  distribution?.ToImmutableList())
        {

        }

        public SemVersion Version { get; }
        public string? Description { get; }
        public IImmutableDictionary<string, string> CustomFields { get; }

        public IImmutableList<UpdateFileInfo> Files { get; set; }
        public IImmutableList<UpdateChangelogInfo> Changelogs { get; set; }
        public IImmutableList<UpdatePackageDistributionInfo> Distributions { get; set; }
    }
}
