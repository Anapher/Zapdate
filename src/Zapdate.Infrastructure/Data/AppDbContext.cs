#pragma warning disable CS8618 // Non-nullable field is uninitialized. Fields are automatically initialized by EF Core

using Zapdate.Core.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

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
