using System.IO;
using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;

namespace Zapdate.Server.Infrastructure.Interfaces
{
    public interface IServerFilesManager
    {
        Task<StoredFile> AddFile(Stream stream);
        Stream ReadFile(Hash fileHash);
    }
}
