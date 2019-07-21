using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Extensions;

namespace Zapdate.Server.Infrastructure.Data.Transactions
{
    public class OrderUpdatePackagesTransaction
    {
        public async Task Execute(IQueryable<UpdatePackage> updatePackages, SemVersion newVersion, SemVersion? previousVersion = null)
        {
            if (previousVersion == null)
            {
                await HandleNewUpdatePackage(updatePackages, newVersion);
                return;
            }

            long lowerVersion;
            long upperVersion;

            if (newVersion > previousVersion)
            {
                upperVersion = newVersion.ToBinaryVersion();
                lowerVersion = previousVersion.ToBinaryVersion();
            }
            else
            {
                lowerVersion = newVersion.ToBinaryVersion();
                upperVersion = previousVersion.ToBinaryVersion();
            }

            var packages = await updatePackages.Where(x => x.IsListed)
                .Where(x => upperVersion >= x.VersionInfo.BinaryVersion && x.VersionInfo.BinaryVersion >= lowerVersion).ToListAsync();

            var firstOrderNumber = packages.Min(x => x.OrderNumber);
            foreach (var updatePackage in packages.OrderBy(x => x.VersionInfo.Version))
                updatePackage.OrderNumber = firstOrderNumber++;
        }

        private async Task HandleNewUpdatePackage(IQueryable<UpdatePackage> updatePackages, SemVersion version)
        {
            // packages that have the same or a higher major and minor
            var packages = await updatePackages.Where(x => x.VersionInfo.BinaryVersion >= version.ToBinaryVersion()).ToListAsync();

            if (packages.Count == 1)
            {
                var package = packages.Single();

                // it is the newest update package
                package.OrderNumber = await GetLatestOrderNumber(updatePackages) + 1;
            }
            else
            {
                var listedPackages = packages.Where(x => x.IsListed);

                int firstOrderNumber;
                if (listedPackages.Any())
                {
                    firstOrderNumber = listedPackages.Min(x => x.OrderNumber);
                }
                else
                {
                    firstOrderNumber = await GetLatestOrderNumber(updatePackages) + 1;
                }

                foreach (var updatePackage in packages.OrderBy(x => x.VersionInfo.Version))
                    updatePackage.OrderNumber = firstOrderNumber++;
            }
        }

        private async Task<int> GetLatestOrderNumber(IQueryable<UpdatePackage> updatePackages)
        {
            return await updatePackages.Where(x => x.OrderNumber >= 0)
                    .OrderByDescending(x => x.OrderNumber).Select(x => (int?)x.OrderNumber).FirstOrDefaultAsync() ?? -1;
        }
    }
}
