using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Dto.UseCaseRequests
{
    public class LoginRequest : IUseCaseRequest<LoginResponse>
    {
        public string? UserName { get; }
        public string? Password { get; }
        public string? RemoteIpAddress { get; }

        public LoginRequest(string? userName, string? password, string? remoteIpAddress)
        {
            UserName = userName;
            Password = password;
            RemoteIpAddress = remoteIpAddress;
        }
    }
}
