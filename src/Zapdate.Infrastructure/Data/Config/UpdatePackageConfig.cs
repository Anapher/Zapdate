using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class UpdatePackageConfig : IEntityTypeConfiguration<UpdatePackage>
    {
        public void Configure(EntityTypeBuilder<UpdatePackage> builder)
        {
            builder.HasIndex(x => x.VersionInfo).IsUnique();
        }
    }
}
