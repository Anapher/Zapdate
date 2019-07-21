using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Server.Core.Domain;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Extensions;

namespace Zapdate.Server.Infrastructure.Data.ObjectQueries
{
    public static class UpdatePackageExtensions
    {
        public static IQueryable<UpdatePackage> IsInProject(this IQueryable<UpdatePackage> list, int projectId)
        {
            return list.Where(x => x.ProjectId == projectId);
        }

        public static IQueryable<UpdatePackage> IsDistributingOnChannel(this IQueryable<UpdatePackage> list, string channel)
        {
            return list.Where(updatePackage => updatePackage.IsListed
                && updatePackage.Distributions.Any(x => x.Name == channel && x.IsDistributing));
        }

        public static IQueryable<UpdatePackage> IsIncludedInVersionFilter(this IQueryable<UpdatePackage> list,
            IList<string> versionFilter)
        {
            return list.Where(x => x.VersionInfo.Prerelease == null || versionFilter.Contains(x.VersionInfo.Prerelease));
        }

        
    }
}
