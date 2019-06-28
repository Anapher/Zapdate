
using System.Threading.Tasks;
using Zapdate.Core.Dto;

namespace Zapdate.Core.Interfaces.Services
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string id, string userName);
    }
}