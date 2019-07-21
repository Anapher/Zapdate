#pragma warning disable CS8618 // Non-nullable field is uninitialized. Fields are automatically initialized by EF Core

using Zapdate.Server.Core.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Infrastructure.Data.Config;

namespace Zapdate.Server.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<UpdatePackage> UpdatePackages { get; set; }
        public DbSet<StoredFile> StoredFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProjectConfig());
            builder.ApplyConfiguration(new UpdatePackageConfig());
            builder.ApplyConfiguration(new UpdateChangelogConfig());
            builder.ApplyConfiguration(new UpdatePackageDistributionConfig());
            builder.ApplyConfiguration(new StoredFileConfig());
        }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity) entry.Entity).CreatedOn = DateTimeOffset.UtcNow;
                }
                ((BaseEntity) entry.Entity).ModifiedOn = DateTimeOffset.UtcNow;
            }
        }
    }
}
