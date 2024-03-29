// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zapdate.Server.Infrastructure.Data;

namespace Zapdate.Server.Infrastructure.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("DistributionChannels");

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.StoredFile", b =>
                {
                    b.Property<string>("FileHash")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CompressedLength");

                    b.Property<long>("Length");

                    b.HasKey("FileHash");

                    b.ToTable("StoredFiles");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdateChangelog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("Language");

                    b.Property<int>("UpdatePackageId");

                    b.HasKey("Id");

                    b.HasIndex("UpdatePackageId", "Language")
                        .IsUnique()
                        .HasFilter("[Language] IS NOT NULL");

                    b.ToTable("UpdateChangelog");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdateFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Hash");

                    b.Property<string>("Path");

                    b.Property<string>("Signature");

                    b.Property<int>("UpdatePackageId");

                    b.HasKey("Id");

                    b.HasIndex("UpdatePackageId");

                    b.ToTable("UpdateFile");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdatePackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("CustomFields");

                    b.Property<string>("Description");

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<int>("OrderNumber");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("OrderNumber");

                    b.ToTable("UpdatePackages");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdatePackageDistribution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsEnforced");

                    b.Property<bool>("IsRolledBack");

                    b.Property<string>("Name");

                    b.Property<DateTimeOffset?>("PublishDate");

                    b.Property<int>("UpdatePackageId");

                    b.HasKey("Id");

                    b.HasIndex("UpdatePackageId", "Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("UpdatePackageDistribution");
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.Project", b =>
                {
                    b.OwnsOne("Zapdate.Server.Core.Domain.Entities.AsymmetricKey", "AsymmetricKey", b1 =>
                        {
                            b1.Property<int>("ProjectId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<bool>("IsPrivateKeyEncrypted");

                            b1.Property<string>("PrivateKey");

                            b1.Property<string>("PublicKey");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Projects");

                            b1.HasOne("Zapdate.Server.Core.Domain.Entities.Project")
                                .WithOne("AsymmetricKey")
                                .HasForeignKey("Zapdate.Server.Core.Domain.Entities.AsymmetricKey", "ProjectId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdateChangelog", b =>
                {
                    b.HasOne("Zapdate.Server.Core.Domain.Entities.UpdatePackage")
                        .WithMany("Changelogs")
                        .HasForeignKey("UpdatePackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdateFile", b =>
                {
                    b.HasOne("Zapdate.Server.Core.Domain.Entities.UpdatePackage")
                        .WithMany("Files")
                        .HasForeignKey("UpdatePackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdatePackage", b =>
                {
                    b.OwnsOne("Zapdate.Server.Core.Domain.Entities.VersionInfo", "VersionInfo", b1 =>
                        {
                            b1.Property<int>("UpdatePackageId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<long>("BinaryVersion")
                                .HasColumnName("VersionBinary");

                            b1.Property<string>("Build")
                                .HasColumnName("VersionBuild");

                            b1.Property<string>("Prerelease")
                                .HasColumnName("VersionPrerelease");

                            b1.Property<string>("Version")
                                .HasColumnName("Version");

                            b1.HasKey("UpdatePackageId");

                            b1.HasIndex("Version")
                                .IsUnique()
                                .HasFilter("[Version] IS NOT NULL");

                            b1.ToTable("UpdatePackages");

                            b1.HasOne("Zapdate.Server.Core.Domain.Entities.UpdatePackage")
                                .WithOne("VersionInfo")
                                .HasForeignKey("Zapdate.Server.Core.Domain.Entities.VersionInfo", "UpdatePackageId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Zapdate.Server.Core.Domain.Entities.UpdatePackageDistribution", b =>
                {
                    b.HasOne("Zapdate.Server.Core.Domain.Entities.UpdatePackage")
                        .WithMany("Distributions")
                        .HasForeignKey("UpdatePackageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
