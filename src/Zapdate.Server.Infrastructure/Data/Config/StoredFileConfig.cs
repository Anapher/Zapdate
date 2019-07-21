using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Server.Core.Domain.Entities;

namespace Zapdate.Server.Infrastructure.Data.Config
{
    public class StoredFileConfig : IEntityTypeConfiguration<StoredFile>
    {
        public void Configure(EntityTypeBuilder<StoredFile> builder)
        {
            builder.HasKey(x => x.FileHash);
        }
    }
}
