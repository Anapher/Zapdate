using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;

namespace Zapdate.Core.Domain.Actions
{
    public interface IAddUpdatePackageFilesAction : IBusinessErrors
    {
        Task AddFiles(UpdatePackage package, IEnumerable<UpdateFileInfo> files, string? keyPassword);
    }

    public class AddUpdatePackageFilesAction : BusinessActionStatus, IAddUpdatePackageFilesAction
    {
        private readonly IStoredFileRepository _filesRepository;
        private readonly IAsymmetricCryptoHandler _asymmetricCryptoHandler;
        private readonly ISymmetricEncryption _symmetricEncryption;
        private readonly IProjectRepository _projectRepository;

        public AddUpdatePackageFilesAction(IStoredFileRepository filesRepository, IProjectRepository projectRepository,
            IAsymmetricCryptoHandler asymmetricCryptoHandler, ISymmetricEncryption symmetricEncryption)
        {
            _filesRepository = filesRepository;
            _asymmetricCryptoHandler = asymmetricCryptoHandler;
            _symmetricEncryption = symmetricEncryption;
            _projectRepository = projectRepository;
        }

        public async Task AddFiles(UpdatePackage package, IEnumerable<UpdateFileInfo> files, string? keyPassword)
        {
            var project = await _projectRepository.GetById(package.ProjectId);
            if (project == null)
            {
                SetError(ResourceNotFoundError.ProjectNotFound(package.ProjectId));
                return;
            }

            var signatureFactory = CreateFileSignatureFactory(project.AsymmetricKey, keyPassword);
            if (signatureFactory == null)
                return; // error

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file.Path))
                {
                    SetError(new FieldValidationError("Files", "The path of a file must not be empty"));
                    return;
                }

                var storedFile = await _filesRepository.FindByHash(file.Hash);
                if (storedFile == null)
                {
                    SetError(new StoredFileNotFoundError(file.Hash));
                    return;
                }

                var signature = signatureFactory(file);
                if (signature == null)
                    return; // error

                var updateFile = new UpdateFile(file.Path, file.Hash.ToString(), signature);
                package.AddFile(updateFile);
            }
        }

        private Func<UpdateFileInfo, string?>? CreateFileSignatureFactory(AsymmetricKey key, string? keyPassword)
        {
            if (key.PrivateKey == null)
            {
                return file =>
                {
                    if (string.IsNullOrEmpty(file.Signature))
                    {
                        SetError(new FieldValidationError("Files", "As the private key is not stored on the server, a signature has to be provided."));
                        return null;
                    }

                    if (!_asymmetricCryptoHandler.VerifyHash(file.Hash, file.Signature, key.PublicKey))
                    {
                        SetError(new FieldValidationError("Files", $"Invalid signature. The signature of file {file.Path} ({file.Hash}) could not be verified."));
                        return null;
                    }

                    return file.Signature!;
                };
            }
            else if (key.IsPrivateKeyEncrypted)
            {
                if (keyPassword == null)
                {
                    SetError(new FieldValidationError("KeyPassword", "The private key is encrypted and no key password was submitted."));
                    return null;
                }

                string privateKey;
                try
                {
                    privateKey = _symmetricEncryption.DecryptString(key.PrivateKey, keyPassword);
                }
                catch (Exception)
                {
                    SetError(new FieldValidationError("KeyPassword", "An error occurred on decrypting private key.", ErrorCode.InvalidKeyPassword));
                    return null;
                }

                return file => _asymmetricCryptoHandler.SignHash(file.Hash, privateKey);
            }
            else
            {
                return file => _asymmetricCryptoHandler.SignHash(file.Hash, key.PrivateKey);
            }
        }
    }
}
