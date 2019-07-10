using System.Collections.Generic;
using Zapdate.Core.Dto.Universal;

namespace Zapdate.Models.Response
{
    public class UpdatePackagePreviewDto
    {
        public string Version { get; set; }
        public string? Description { get; set; }
        public IList<UpdatePackageDistributionInfo> Distribution { get; set; }

        public long UpdateSize { get; set; }

        public int NewFiles { get; set; }
        public int DeletedFiles { get; set; }
        public int UpdatedFiles { get; set; }

        public int DeployedMachines { get; set; }
        public int Downloads { get; set; }
    }
}
