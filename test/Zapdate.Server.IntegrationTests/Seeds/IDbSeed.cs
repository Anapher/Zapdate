using Zapdate.Server.Infrastructure.Data;
using Zapdate.Server.Infrastructure.Identity;

namespace Zapdate.Server.IntegrationTests.Seeds
{
    public interface IDbSeed
    {
        void PopulateTestData(AppIdentityDbContext dbContext);
        void PopulateTestData(AppDbContext dbContext);
    }
}
