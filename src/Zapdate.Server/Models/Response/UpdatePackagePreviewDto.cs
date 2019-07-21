using System.Collections.Generic;
using Zapdate.Server.Core.Dto.Universal;

namespace Zapdate.Server.Models.Response
{
    public class UpdatePackagePreviewDto
    {
        public UpdatePackagePreviewDto(string version, string description, IList<UpdatePackageDistributionInfo> distribution)
        {
            Version = version;
            Description = description;
            Distribution = distribution;
        }

        public string Version { get; }
        public string? Description { get; }
        public IList<UpdatePackageDistributionInfo> Distribution { get; }

        public long UpdateSize { get; set; }

        public int NewFiles { get; set; }
        public int DeletedFiles { get; set; }
        public int UpdatedFiles { get; set; }

        public int DeployedMachines { get; set; }
        public int Downloads { get; set; }
    }
}
