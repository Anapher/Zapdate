using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zapdate.Core.Domain.Entities;

namespace Zapdate.Infrastructure.Data.Config
{
    public class StoredFileConfig : IEntityTypeConfiguration<StoredFile>
    {
        public void Configure(EntityTypeBuilder<StoredFile> builder)
        {
            builder.HasKey(x => x.FileHash);
        }
    }
}
