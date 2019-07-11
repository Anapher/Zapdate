using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Immutable;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.OwnsOne(x => x.AsymmetricKey);
            builder.Property(x => x.DistributionChannels).HasConversion(x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<IImmutableList<string>>(x)).IsRequired();
        }
    }
}
