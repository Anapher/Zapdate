using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Infrastructure.Data.Transactions;

namespace Zapdate.Infrastructure.Data.Repositories
{
    public class UpdatePackageRepository : EfRepository<UpdatePackage>, IUpdatePackageRepository
    {
        protected UpdatePackageRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task OrderUpdatePackages(int projectId, SemVersion newVersion, SemVersion? previousVersion = null)
        {
            var updatePackages = _appDbContext.UpdatePackages.Where(x => x.ProjectId == projectId);

            var transaction = new OrderUpdatePackagesTransaction();
            await transaction.Execute(updatePackages, newVersion, previousVersion);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
