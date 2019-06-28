using System.Security.Claims;

namespace Zapdate.Core.Interfaces.Services
{
    public interface IJwtValidator
    {
        ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}