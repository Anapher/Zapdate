using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Collections.Immutable;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    internal class UpdatePackageConfig : IEntityTypeConfiguration<UpdatePackage>
    {
        public void Configure(EntityTypeBuilder<UpdatePackage> builder)
        {
            builder.HasIndex(x => x.OrderNumber); // not unique to make swaps work

            builder.Property(x => x.CustomFields).HasConversion(x => JsonConvert.SerializeObject(x),
                x => JsonConvert.DeserializeObject<IImmutableDictionary<string, string>>(x));

            builder.OwnsOne(x => x.VersionInfo, a =>
            {
                a.Property(p => p.BinaryVersion)
                    .HasColumnName("VersionBinary");
                a.Property(p => p.Build)
                    .HasColumnName("VersionBuild");
                a.Property(p => p.Prerelease)
                   .HasColumnName("VersionPrerelease");
                a.Property(p => p.Version)
                   .HasColumnName("Version");

                a.HasIndex(x => x.Version).IsUnique();
            });

            builder.Metadata.FindNavigation(nameof(UpdatePackage.Changelogs)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(UpdatePackage.Files)).SetPropertyAccessMode(PropertyAccessMode.Field);
            builder.Metadata.FindNavigation(nameof(UpdatePackage.Distributions)).SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
