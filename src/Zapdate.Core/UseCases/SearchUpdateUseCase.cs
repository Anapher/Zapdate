using System.Linq;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Interfaces;
using Zapdate.Core.Interfaces.Gateways.Repositories;
using Zapdate.Core.Interfaces.UseCases;
using Zapdate.Core.Specifications.UpdatePackage;

namespace Zapdate.Core.UseCases
{
    public class SearchUpdateUseCase : UseCaseStatus<SearchUpdateResponse>, ISearchUpdateUseCase
    {
        private readonly IUpdatePackageRepository _repository;

        public SearchUpdateUseCase(IUpdatePackageRepository repository)
        {
            _repository = repository;
        }

        public async Task<SearchUpdateResponse?> Handle(SearchUpdateRequest message)
        {
            var isRolledback = false;
            var isEnforced = false;

            var updatePackage = await _repository.GetFirstOrDefaultBySpecs(new ProjectSpec(message.ProjectId),
                new VersionSpec(message.Version), new IncludeDistributionsSpec());
            if (updatePackage == null)
            {
                //var nearestPackage = await FindNearestUpdatePackage(message.ProjectId, message.Version, _repository);

                //if (nearestPackage == null)
                //{
                //    // seems like there aren't any update packages in the project, or at least none
                //    // with a smaller version
                //    return SearchUpdateResponse.None;
                //}

                // we don't check for rolled back or enforced as the user doesn't have the exact update package
            }
            else
            {
                var channelInfo = updatePackage.Distributions.FirstOrDefault(x => x.Name == message.Channel);
                if (channelInfo != null)
                {
                    isRolledback = channelInfo.IsRolledBack;
                    isEnforced = channelInfo.IsEnforced;
                }
            }

            var latestPackage = await _repository.GetLatestBySpecs(new ProjectSpec(message.ProjectId),
                new DistributingOnChannelSpec(message.Channel), new VersionFilterSpec(message.VersionFilter));

            if (latestPackage == null)
            {
                return SearchUpdateResponse.None; // seems like there aren't any packages published on this channel
            }

            if (latestPackage.VersionInfo.SemVersion > message.Version)
            {
                // new update
                return new SearchUpdateResponse(updatePackage, latestPackage, isEnforced, isRolledback);
            }

            if (isRolledback)
            {
                // downgrade to latest version
                return new SearchUpdateResponse(updatePackage, latestPackage, isEnforced, isRolledback);
            }

            // no updates
            return SearchUpdateResponse.None;
        }

        private static async Task<UpdatePackage?> FindNearestUpdatePackage(int projectId, SemVersion version,
            IUpdatePackageRepository repository)
        {
            // select all packages with the same binary version
            var relevantVersions = await repository.GetAllBySpecs(new ProjectSpec(projectId),
                new HasSameBinaryVersionSpec(version));

            if (relevantVersions.Any())
            {
                // find the last package that is below our version
                var previousPackage = relevantVersions.OrderBy(x => x.OrderNumber)
                    .TakeWhile(x => x.VersionInfo.SemVersion < version).LastOrDefault();

                if (previousPackage != null)
                    return previousPackage;

                // as the lowest update package with the same binary version comes after our version,
                // we know that we need to select the highest update package with a binary version below
            }

            // it is the highest update package below the binary version (as no packages with this binary version exist)
            return await repository.GetLatestBySpecs(new ProjectSpec(projectId), new HasLowerBinaryVersionSpec(version));
        }
    }
}
