using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Server.Core.Domain.Actions;
using Zapdate.Server.Core.Domain.Entities;
using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Errors;
using Zapdate.Server.Core.Interfaces;
using Zapdate.Server.Core.Interfaces.Gateways.Repositories;
using Zapdate.Server.Core.Interfaces.UseCases;
using Zapdate.Server.Core.Specifications;
using Zapdate.Server.Core.Specifications.UpdatePackage;

namespace Zapdate.Server.Core.UseCases
{
    public class CreateUpdatePackageUseCase : UseCaseStatus<CreateUpdatePackageResponse>, ICreateUpdatePackageUseCase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUpdatePackageRepository _updatePackageRepository;
        private readonly IAddUpdatePackageFilesAction _addFilesAction;

        public CreateUpdatePackageUseCase(IProjectRepository projectRepository, IUpdatePackageRepository updatePackageRepository,
            IAddUpdatePackageFilesAction addFilesAction)
        {
            _projectRepository = projectRepository;
            _updatePackageRepository = updatePackageRepository;
            _addFilesAction = addFilesAction;
        }

        public async Task<CreateUpdatePackageResponse?> Handle(CreateUpdatePackageRequest message)
        {
            var project = await _projectRepository.GetById(message.ProjectId);
            if (project == null)
                return ReturnError(ResourceNotFoundError.ProjectNotFound(message.ProjectId));

            var package = message.UpdatePackage;

            var updatePackage = new UpdatePackage(package.Version, message.ProjectId);
            updatePackage.Description = package.Description;
            updatePackage.CustomFields = package.CustomFields;

            if (!CopyChangelogs(updatePackage, package.Changelogs))
                return null; // error

            await _addFilesAction.AddFiles(updatePackage, package.Files, message.AsymmetricKeyPassword);
            if (InheritError(_addFilesAction))
                return null; // error

            CopyDistributions(updatePackage, package.Distributions);

            try
            {
                await _updatePackageRepository.Add(updatePackage);
            }
            catch (Exception)
            {
                if (await _updatePackageRepository.GetFirstOrDefaultBySpecs(new ProjectSpec(project.Id),
                    new VersionSpec(updatePackage.VersionInfo.SemVersion)) != null)
                {
                    SetError(new UpdatePackageAlreadyExistsError());
                    return null;
                }
                throw;
            }

            await _updatePackageRepository.OrderUpdatePackages(message.ProjectId, updatePackage.VersionInfo.SemVersion);

            return new CreateUpdatePackageResponse(updatePackage.Id);
        }

        private void CopyDistributions(UpdatePackage package, IEnumerable<UpdatePackageDistributionInfo> distributions)
        {
            foreach (var distributionDto in distributions)
            {
                var distribution = package.AddDistribution(distributionDto.Name);
                if (distributionDto.PublishDate != null)
                    distribution.Publish(distributionDto.PublishDate);
            }
        }

        private bool CopyChangelogs(UpdatePackage package, IEnumerable<UpdateChangelogInfo> changelogs)
        {
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
    }
}
