using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class UpdateChangelogConfig : IEntityTypeConfiguration<UpdateChangelog>
    {
        public void Configure(EntityTypeBuilder<UpdateChangelog> builder)
        {
            builder.HasIndex(x => new { x.UpdatePackageId, x.Language }).IsUnique();
        }
    }
}
