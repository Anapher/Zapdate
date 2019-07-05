using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeElements.Core;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Interfaces.Gateways.Repositories;

namespace Zapdate.Infrastructure.Data.Repositories
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
