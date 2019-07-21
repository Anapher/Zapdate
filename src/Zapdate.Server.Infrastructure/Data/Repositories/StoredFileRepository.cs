using System.Threading.Tasks;
using Zapdate.Core;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;

namespace Zapdate.Server.Infrastructure.Data.Repositories
{
    public class StoredFileRepository : IStoredFileRepository
    {
        private readonly AppDbContext _context;

        public StoredFileRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<StoredFile> FindByHash(Hash hash)
        {
            return _context.StoredFiles.FindAsync(hash.ToString());
        }
    }
}
