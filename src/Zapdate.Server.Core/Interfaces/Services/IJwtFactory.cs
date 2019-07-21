
using System.Threading.Tasks;
using Zapdate.Server.Core.Dto;

namespace Zapdate.Server.Core.Interfaces.Services
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string id, string userName);
    }
}