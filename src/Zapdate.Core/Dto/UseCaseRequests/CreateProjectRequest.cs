using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core.Dto.UseCaseRequests
{
    public class CreateProjectRequest : IUseCaseRequest<CreateProjectResponse>
    {
        public CreateProjectRequest(string projectName, RsaKeyStorage rsaKeyStorage, string rsaKeyPassword)
        {
            ProjectName = projectName;
            RsaKeyStorage = rsaKeyStorage;
            RsaKeyPassword = rsaKeyPassword;
        }

        public string ProjectName { get; }
        public RsaKeyStorage RsaKeyStorage { get; }
        public string? RsaKeyPassword { get; }
    }


    public enum RsaKeyStorage
    {
        Server,
        ServerEncrypted,
        Locally
    }
}
