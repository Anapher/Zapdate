using AutoMapper;
using Zapdate.Server.Core.Domain.Entities;
using System.Linq;
using System.Reflection;

namespace Zapdate.Server.Infrastructure.Identity.Mapping
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            var refreshTokensField = typeof(User).GetField("_refreshTokens", BindingFlags.NonPublic | BindingFlags.Instance);
            CreateMap<AppUser, User>().ConstructUsing(u => new User(u.Id, u.UserName, u.PasswordHash)).AfterMap((appUser, user) => refreshTokensField.SetValue(user, appUser.RefreshTokens.ToList()));
            CreateMap<User, AppUser>().AfterMap((user, appUser) => appUser.RefreshTokens = user.RefreshTokens.ToList());
        }
    }
}
