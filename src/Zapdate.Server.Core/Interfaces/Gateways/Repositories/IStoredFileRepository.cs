using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;

namespace Zapdate.Server.Core.Interfaces.Gateways.Repositories
{
    public interface IStoredFileRepository
    {
        Task<StoredFile> FindByHash(Hash hash);
    }
}
