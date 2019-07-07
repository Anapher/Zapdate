using System.IO;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Interfaces
{
    public interface IServerFilesManager
    {
        Task<StoredFile> AddFile(Stream stream);
        Stream ReadFile(Hash fileHash);
    }
}
