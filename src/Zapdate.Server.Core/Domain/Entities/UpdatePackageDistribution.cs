using System;

namespace Zapdate.Server.Core.Domain.Entities
{
    public class UpdatePackageDistribution
    {
        public UpdatePackageDistribution(string name, int updatePackageId = 0)
        {
            Name = name;
            UpdatePackageId = updatePackageId;
        }

#pragma warning disable CS8618 // Constructor for mapping
        private UpdatePackageDistribution()
        {
        }
#pragma warning restore CS8618

        public int Id { get; private set; }
        public int UpdatePackageId { get; private set; }
        public string Name { get; private set; }

        public DateTimeOffset? PublishDate { get; private set; }
        public bool IsRolledBack { get; private set; }
        public bool IsEnforced { get; set; }

        public bool IsPublished => PublishDate != null;
        public bool IsDistributing => !IsRolledBack && DateTimeOffset.UtcNow >= PublishDate;

        public void Publish(DateTimeOffset? publishOn = null)
        {
            IsRolledBack = false;
            PublishDate = publishOn ?? DateTimeOffset.UtcNow;
        }

        public void Rollback()
        {
            IsRolledBack = true;
        }

        public void Unpublish()
        {
            IsRolledBack = false;
            PublishDate = null;
        }
    }
}
