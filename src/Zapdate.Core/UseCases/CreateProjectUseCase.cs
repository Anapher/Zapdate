using System.Threading.Tasks;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Services;
using Zapdate.Core.Interfaces.UseCases;

namespace Zapdate.Core.UseCases
{
    public class CreateProjectUseCase : UseCaseStatus<CreateProjectResponse>, ICreateProjectUseCase
    {
        private readonly IAsymmetricKeyFactory _asymmetricKeyFactory;

        public CreateProjectUseCase(IAsymmetricKeyFactory asymmetricKeyFactory)
        {
            _asymmetricKeyFactory = asymmetricKeyFactory;
        }

        public async Task<CreateProjectResponse?> Handle(CreateProjectRequest message)
        {
            if (string.IsNullOrWhiteSpace(message.ProjectName))
                return ReturnError(new FieldValidationError(nameof(message.ProjectName), "The project name must not be empty."));

            if (message.RsaKeyStorage == RsaKeyStorage.ServerEncrypted && (string.IsNullOrEmpty(message.RsaKeyPassword) || message.RsaKeyPassword!.Length < 6))
                return ReturnError(new FieldValidationError(nameof(message.RsaKeyPassword), "The RSA key password is required."));

            var key = _asymmetricKeyFactory.Create();

        }
    }
}
