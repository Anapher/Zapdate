using System;

namespace Zapdate.Server.Core.Dto.Universal
{
    public class UpdatePackageDistributionInfo
    {
        public UpdatePackageDistributionInfo(string name, DateTimeOffset? publishDate)
        {
            Name = name;
            PublishDate = publishDate;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdatePackageDistributionInfo()
        {
        }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public DateTimeOffset? PublishDate { get; private set; }
        public bool IsRolledBack { get; set; }
        public bool IsEnforced { get; set; }
    }
}
