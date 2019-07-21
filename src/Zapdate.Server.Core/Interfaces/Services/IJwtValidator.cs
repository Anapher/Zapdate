using System.Security.Claims;

namespace Zapdate.Server.Core.Interfaces.Services
{
    public interface IJwtValidator
    {
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}