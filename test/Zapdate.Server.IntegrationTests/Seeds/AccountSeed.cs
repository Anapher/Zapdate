using System;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Infrastructure.Data;
using Zapdate.Server.Infrastructure.Identity;

namespace Zapdate.Server.IntegrationTests.Seeds
{
    public class AccountSeed : IDbSeed
    {
        public void PopulateTestData(AppIdentityDbContext dbContext)
        {
            var user = new AppUser
            {
                Id = "41532945-599e-4910-9599-0e7402017fbe",
                UserName = "mmacneil",
                NormalizedUserName = "MMACNEIL",
                Email = "mark@fullstackmark.com",
                NormalizedEmail = "MARK@FULLSTACKMARK.COM",
                PasswordHash = "$2y$12$ahDJbfA1Tk/.8SZKzyX1keNJriD.hi2cOxhi1fRIG8HiRNab1jNn6",
                SecurityStamp = "YIJZLWUFIIDD3IZSFDD7OQWG6D4QIYPB",
                ConcurrencyStamp = "e432007d-0a54-4332-9212-ca9d7e757275",
            };
            user.RefreshTokens.Add(new RefreshToken("rB1afdEe6MWu6TyN8zm58xqt/3KWOLRAah2nHLWcboA=", DateTimeOffset.UtcNow.AddDays(6), "127.0.0.1"));

            dbContext.Users.Add(user);

            dbContext.SaveChanges();
        }

        public void PopulateTestData(AppDbContext dbContext)
        {
        }
    }
}
