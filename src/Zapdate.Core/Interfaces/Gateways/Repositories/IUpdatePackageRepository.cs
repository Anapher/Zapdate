using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Core.Interfaces.Gateways.Repositories
{
    public interface IUpdatePackageRepository : IRepository<UpdatePackage>
    {
        Task OrderUpdatePackages(int projectId, SemVersion newVersion, SemVersion? previousVersion = null);
    }
}
