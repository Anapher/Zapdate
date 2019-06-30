using System;
using System.Threading.Tasks;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.Services;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;
using Zapdate.Core.Interfaces.UseCases;

namespace Zapdate.Core.UseCases
{
    public class CreateProjectUseCase : UseCaseStatus<CreateProjectResponse>, ICreateProjectUseCase
    {
        private readonly IAsymmetricKeyParametersFactory _asymmetricKeyFactory;
        private readonly IProjectRepository _repository;
        private readonly ISymmetricEncryption _encryption;

        public CreateProjectUseCase(IAsymmetricKeyParametersFactory asymmetricKeyFactory, IProjectRepository repository, ISymmetricEncryption encryption)
        {
            _asymmetricKeyFactory = asymmetricKeyFactory;
            _repository = repository;
            _encryption = encryption;
        }

        public async Task<CreateProjectResponse?> Handle(CreateProjectRequest message)
        {
            if (string.IsNullOrWhiteSpace(message.ProjectName))
                return ReturnError(new FieldValidationError(nameof(message.ProjectName), "The project name must not be empty."));

            if (message.KeyStorage == KeyStorage.ServerEncrypted && (string.IsNullOrEmpty(message.KeyPassword) || message.KeyPassword!.Length < 6))
                return ReturnError(new FieldValidationError(nameof(message.KeyPassword), "The key password is required."));

            var key = _asymmetricKeyFactory.Create();
            var asymmetricKey = CreateAsymmetricKey(key, message.KeyStorage, message.KeyPassword);

            var project = new Project(message.ProjectName, asymmetricKey);
            var result = await _repository.Add(project);

            if (message.KeyStorage == KeyStorage.Locally)
            {
                return new CreateProjectResponse(result.Id, key.PrivateKey);
            }
            else
            {
                return new CreateProjectResponse(result.Id);
            }
        }

        private AsymmetricKey CreateAsymmetricKey(AsymmetricKeyParameters parameters, KeyStorage storage, string? password)
        {
            switch (storage)
            {
                case KeyStorage.Server:
                    return new AsymmetricKey(parameters.PublicKey, parameters.PrivateKey);
                case KeyStorage.ServerEncrypted:
                    var encryptedPrivateKey = _encryption.EncryptString(parameters.PrivateKey, password!);
                    return new AsymmetricKey(parameters.PublicKey, encryptedPrivateKey, true);
                case KeyStorage.Locally:
                    return new AsymmetricKey(parameters.PublicKey);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
