using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Server.Core.Domain.Entities;

namespace Zapdate.Server.Infrastructure.Data.Config
{
    internal class UpdateChangelogConfig : IEntityTypeConfiguration<UpdateChangelog>
    {
        public void Configure(EntityTypeBuilder<UpdateChangelog> builder)
        {
            builder.HasIndex(x => new { x.UpdatePackageId, x.Language }).IsUnique();
        }
    }
}
