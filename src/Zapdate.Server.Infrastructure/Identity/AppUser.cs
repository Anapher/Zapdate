using Zapdate.Server.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Zapdate.Server.Infrastructure.Identity
{
    public class AppUser : IdentityUser
    {
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public string? UnconfirmedEmailAddress { get; set; }
    }
}