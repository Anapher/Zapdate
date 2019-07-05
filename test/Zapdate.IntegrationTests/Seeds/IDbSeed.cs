using Zapdate.Infrastructure.Data;
using Zapdate.Infrastructure.Identity;

namespace Zapdate.IntegrationTests.Seeds
{
    public interface IDbSeed
    {
        void PopulateTestData(AppIdentityDbContext dbContext);
        void PopulateTestData(AppDbContext dbContext);
    }
}
