using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Actions;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Errors;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Core.Specifications;

namespace Zapdate.Core.UseCases
{
    public class PatchUpdatePackageUseCase : UseCaseStatus<PatchUpdatePackageResponse>, IPatchUpdatePackageUseCase
    {
        private readonly IUpdatePackageRepository _updatePackageRepository;
        private readonly IAddUpdatePackageFilesAction _addFilesAction;

        public PatchUpdatePackageUseCase(IUpdatePackageRepository updatePackageRepository, IAddUpdatePackageFilesAction addFilesAction)
        {
            _updatePackageRepository = updatePackageRepository;
            _addFilesAction = addFilesAction;
        }

        public async Task<PatchUpdatePackageResponse?> Handle(PatchUpdatePackageRequest message)
        {
            var updatePackage = await _updatePackageRepository.GetSingleBySpec(new FullUpdatePackageVersionSpec(message.CurrentVersion, message.ProjectId));
            if (updatePackage == null)
            {
                return ReturnError(ResourceNotFoundError.UpdatePackageNotFound(message.ProjectId, message.CurrentVersion.ToString(false)));
            }

            await UpdateFiles(updatePackage, message.UpdatePackage, message.AsymmetricKeyPassword);
            if (HasError)
                return default;

            UpdateDescription(updatePackage, message.UpdatePackage);
            UpdateCustomFields(updatePackage, message.UpdatePackage);
            UpdateChangelogs(updatePackage, message.UpdatePackage);
            UpdateDistributions(updatePackage, message.UpdatePackage);

            var isVersionUpdated = UpdateVersion(updatePackage, message.UpdatePackage);

            await _updatePackageRepository.Update(updatePackage);

            if (isVersionUpdated)
            {
                await _updatePackageRepository.OrderUpdatePackages(message.ProjectId, updatePackage.VersionInfo.SemVersion, message.CurrentVersion);
            }

            return new PatchUpdatePackageResponse();
        }

        private bool UpdateVersion(UpdatePackage target, UpdatePackageInfo source)
        {
            if (source.Version != target.VersionInfo.SemVersion)
            {
                if (source.Version.ToString(false) == target.VersionInfo.Version)
                {
                    // only build updated
                    target.VersionInfo.Build = source.Version.Build;
                }
                else
                {
                    target.UpdateVersion(source.Version);
                    return true;
                }
            }

            return false;
        }

        private void UpdateDescription(UpdatePackage target, UpdatePackageInfo source)
        {
            target.Description = source.Description;
        }

        private void UpdateCustomFields(UpdatePackage target, UpdatePackageInfo source)
        {
            if (target.CustomFields.OrderBy(x => x.Key).SequenceEqual(source.CustomFields.OrderBy(x => x.Key)))
                return;

            target.CustomFields = source.CustomFields;
        }

        private void UpdateChangelogs(UpdatePackage target, UpdatePackageInfo source)
        {
            PatchList(target.Changelogs, source.Changelogs, (x, y) => x.Language == y.Language,
                x => target.RemoveChangelog(x.Language), x => target.AddChangelog(x.Language, x.Content),
                (x, y) => x.Content = y.Content);
        }

        private void UpdateDistributions(UpdatePackage target, UpdatePackageInfo source)
        {
            void UpdateItem(UpdatePackageDistribution entity, UpdatePackageDistributionInfo dto)
            {
                if (dto.IsRolledBack) entity.Rollback();
                else if (dto.PublishDate != null) entity.Publish(dto.PublishDate);
                else entity.Unpublish();

                entity.IsEnforced = dto.IsEnforced;
            }

            PatchList(target.Distributions, source.Distributions, (x, y) => x.Name == y.Name,
                x => target.RemoveDistribution(x),
                x =>
                {
                    var distribution = target.AddDistribution(x.Name);
                    UpdateItem(distribution, x);
                },
                UpdateItem);
        }

        private async Task UpdateFiles(UpdatePackage target, UpdatePackageInfo source, string? keyPassword)
        {
            var newFiles = new List<UpdateFileInfo>();
            var oldFiles = target.Files.ToList();

            PatchList(target.Files, source.Files, (x, y) => Hash.Parse(x.Hash).Equals(y.Hash) && x.Path == y.Path, x => target.RemoveFile(x),
                x => newFiles.Add(x), (_, __) => { /* updates not supported */ });

            // find files in this package with equal hash to copy the signature (e. g. if just the path was changed)
            foreach (var newFile in newFiles.ToList())
            {
                var existingFile = oldFiles.FirstOrDefault(x => x.Hash == newFile.Hash.ToString());
                if (existingFile != null)
                {
                    target.AddFile(new UpdateFile(newFile.Path, newFile.Hash.ToString(), existingFile.Signature));
                    newFiles.Remove(newFile);
                }
            }

            if (!newFiles.Any()) // completed without calculating signatures
                return;

            await _addFilesAction.AddFiles(target, newFiles, keyPassword);
            InheritError(_addFilesAction);
        }

        private static void PatchList<TEntity, TDto>(IEnumerable<TEntity> entities, IEnumerable<TDto>? dtos, Func<TEntity, TDto, bool> compare,
            Action<TEntity> removeItem, Action<TDto> addItem, Action<TEntity, TDto> updateItem)
        {
            if (dtos == null)
            {
                // clear
                foreach (var entity in entities)
                    removeItem(entity);

                return;
            }

            // new or modified
            foreach (var dto in dtos)
            {
                var existing = entities.FirstOrDefault(x => compare(x, dto));
                if (existing != null)
                    updateItem(existing, dto);
                else
                    addItem(dto);
            }

            // deleted
            foreach (var entity in entities.Where(x => !dtos.Any(y => compare(x, y))).ToList())
            {
                removeItem(entity);
            }
        }
    }
}
