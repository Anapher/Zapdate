using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.OwnsOne(x => x.AsymmetricKey);
        }
    }
}
