using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class UpdatePackageDistributionConfig : IEntityTypeConfiguration<UpdatePackageDistribution>
    {
        public void Configure(EntityTypeBuilder<UpdatePackageDistribution> builder)
        {
            builder.HasIndex(x => new { x.UpdatePackageId, x.Name }).IsUnique();
        }
    }
}
