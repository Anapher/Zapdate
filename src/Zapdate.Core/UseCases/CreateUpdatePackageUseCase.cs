using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.Services;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Core.Specifications;

namespace Zapdate.Core.UseCases
{
    public class CreateUpdatePackageUseCase : UseCaseStatus<CreateUpdatePackageResponse>, ICreateUpdatePackageUseCase
    {
        private readonly IStoredFileRepository _filesRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAsymmetricCryptoHandler _asymmetricCryptoHandler;
        private readonly ISymmetricEncryption _symmetricEncryption;
        private readonly IUpdatePackageRepository _updatePackageRepository;

        public CreateUpdatePackageUseCase(IStoredFileRepository filesRepository, IProjectRepository projectRepository,
            IAsymmetricCryptoHandler asymmetricCryptoHandler, ISymmetricEncryption symmetricEncryption,
            IUpdatePackageRepository updatePackageRepository)
        {
            _filesRepository = filesRepository;
            _projectRepository = projectRepository;
            _asymmetricCryptoHandler = asymmetricCryptoHandler;
            _symmetricEncryption = symmetricEncryption;
            _updatePackageRepository = updatePackageRepository;
        }

        public async Task<CreateUpdatePackageResponse?> Handle(CreateUpdatePackageRequest message)
        {
            var project = await _projectRepository.GetById(message.ProjectId);
            if (project == null)
                return ReturnError(ResourceNotFoundError.ProjectNotFound(message.ProjectId));

            var updatePackage = new UpdatePackage(message.Version);
            updatePackage.Description = message.Description;

            foreach (var customField in message.CustomFields)
                updatePackage.CustomFields.Add(customField);

            if (!CopyChangelogs(updatePackage, message.Changelogs))
                return null; // error

            if (!await AddFiles(updatePackage, message.Files, project.AsymmetricKey, message.AsymmetricKeyPassword))
                return null; // error

            CopyDistributions(updatePackage, message.Distributions);

            try
            {
                await _updatePackageRepository.Add(updatePackage);
            }
            catch (Exception)
            {
                if (await _updatePackageRepository.GetSingleBySpec(new UpdatePackageVersionSpec(updatePackage.VersionInfo.SemVersion)) != null)
                {
                    SetError(new UpdatePackageAlreadyExistsError());
                    return null;
                }
                throw;
            }

            await _updatePackageRepository.OrderUpdatePackages(message.ProjectId, updatePackage.VersionInfo.SemVersion);

            return new CreateUpdatePackageResponse(updatePackage.Id);
        }

        private void CopyDistributions(UpdatePackage package, IReadOnlyList<UpdatePackageDistributionDto>? distributions)
        {
            if (distributions == null)
                return;

            foreach (var distributionDto in distributions)
            {
                var distribution = package.AddDistribution(distributionDto.Name);
                if (distributionDto.PublishDate != null)
                    distribution.Publish(distributionDto.PublishDate);
            }
        }

        private bool CopyChangelogs(UpdatePackage package, IReadOnlyList<UpdateChangelogDto>? changelogs)
        {
            if (changelogs == null)
                return true;

            foreach (var changelog in changelogs)
            {
                if (package.Changelogs.Any(x => x.Language.Equals(changelog.Language, StringComparison.OrdinalIgnoreCase)))
                {
                    SetError(new FieldValidationError("Changelogs", $"The changelogs must have distinct languages (duplicate: {changelog.Language})."));
                    return false;
                }

                try
                {
                    package.AddChangelog(changelog.Language, changelog.Content);
                }
                catch (CultureNotFoundException)
                {
                    SetError(new FieldValidationError("Changeogs", $"The language {changelog.Language} was not found."));
                    return false;
                }
            }

            return true;
        }

        private async Task<bool> AddFiles(UpdatePackage package, IEnumerable<UpdateFileDto> files, AsymmetricKey key, string? keyPassword)
        {
            var signatureFactory = CreateFileSignatureFactory(key, keyPassword);
            if (signatureFactory == null)
                return false; // error

            foreach (var file in files)
            {
                if (string.IsNullOrEmpty(file.Path))
                {
                    SetError(new FieldValidationError("Files", "The path of a file must not be empty"));
                    return false;
                }

                var storedFile = await _filesRepository.FindByHash(file.Hash);
                if (storedFile == null)
                {
                    SetError(new StoredFileNotFoundError(file.Hash));
                    return false;
                }

                var signature = signatureFactory(file);
                if (signature == null)
                    return false; // error

                var updateFile = new UpdateFile(file.Path, file.Hash.ToString(), signature);
                package.AddFile(updateFile);
            }

            return true;
        }


        private Func<UpdateFileDto, string?>? CreateFileSignatureFactory(AsymmetricKey key, string? keyPassword)
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
