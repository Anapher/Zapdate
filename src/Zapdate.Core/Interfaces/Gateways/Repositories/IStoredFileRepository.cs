using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Core.Interfaces.Gateways.Repositories
{
    public interface IStoredFileRepository
    {
        Task<StoredFile> FindByHash(Hash hash);
    }
}
