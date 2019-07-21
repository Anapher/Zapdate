using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Dto.UseCaseRequests
{
    public class CreateProjectRequest : IUseCaseRequest<CreateProjectResponse>
    {
        public CreateProjectRequest(string projectName, KeyStorage keyStorage, string? keyPassword)
        {
            ProjectName = projectName;
            KeyStorage = keyStorage;
            KeyPassword = keyPassword;
        }

        public string ProjectName { get; }
        public KeyStorage KeyStorage { get; }
        public string? KeyPassword { get; }
    }


    public enum KeyStorage
    {
        Server,
        ServerEncrypted,
        Locally
    }
}
