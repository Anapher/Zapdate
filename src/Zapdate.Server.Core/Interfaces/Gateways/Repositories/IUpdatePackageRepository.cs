using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;

namespace Zapdate.Server.Core.Interfaces.Gateways.Repositories
{
    public interface IUpdatePackageRepository : IRepository<UpdatePackage>
    {
        Task OrderUpdatePackages(int projectId, SemVersion newVersion, SemVersion? previousVersion = null);

        Task<UpdatePackage?> GetLatestBySpecs(params ISpecification<UpdatePackage>[] specs);
    }
}
