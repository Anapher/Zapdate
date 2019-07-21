using System.Collections.Generic;

namespace Zapdate.Server.Models.Response
{
    public class ProjectDto
    {
        public int Id { get; }
        public string Name { get; }
        public IEnumerable<string> DistributionChannels { get; }

        public ProjectDto(int id, string name, IEnumerable<string> distributionChannels)
        {
            Id = id;
            Name = name;
            DistributionChannels = distributionChannels;
        }
    }
}
