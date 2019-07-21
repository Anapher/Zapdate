#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.

using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Infrastructure.Data.Transactions;

namespace Zapdate.Server.Infrastructure.Data.Repositories
{
    public class UpdatePackageRepository : EfRepository<UpdatePackage>, IUpdatePackageRepository
    {
        public UpdatePackageRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task OrderUpdatePackages(int projectId, SemVersion newVersion, SemVersion? previousVersion = null)
        {
            var updatePackages = _appDbContext.UpdatePackages.Where(x => x.ProjectId == projectId);

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(updatePackages, newVersion, previousVersion);
            await _appDbContext.SaveChangesAsync();
        }

        public Task<UpdatePackage?> GetLatestBySpecs(params ISpecification<UpdatePackage>[] specs)
        {
            var query = QuerySpecs(specs);
            return query.OrderByDescending(x => x.OrderNumber).FirstOrDefaultAsync();
        }
    }
}
