using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zapdate.Models.Request
{
    public class CreateUpdatePackageRequest
    {
        public string Version { get; set; }
        public string? Description { get; set; }
        public IDictionary<string, string>? CustomFields { get; set; }

        public IList<UpdateFileDto> Files { get; set; }
        public IList<UpdateChangelogDto> Changelogs { get; set; }
    }

    public class UpdateChannelDto
    {
        public string Name { get; set; }
        public DateTimeOffset? PublishDate { get; set; }
    }

    public class UpdateChangelogDto
    {
        public string Language { get; set; }
        public string Content { get; set; }
    }

    public class UpdateFileDto
    {
        public string Path { get; set; }
        public string Sha256Checksum { get; set; }
        public string? Signature { get; set; }
    }
}
