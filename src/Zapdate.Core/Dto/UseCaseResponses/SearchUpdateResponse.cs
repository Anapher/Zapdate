using Zapdate.Core.Domain.Entities;

namespace Zapdate.Core.Dto.UseCaseResponses
{
    public class SearchUpdateResponse
    {
        public SearchUpdateResponse(UpdatePackage? sourcePackage, UpdatePackage? recommendedPackage,
            bool isEnforced, bool isRollback)
        {
            SourcePackage = sourcePackage;
            RecommendedPackage = recommendedPackage;
            IsEnforced = isEnforced;
            IsRollback = isRollback;
        }

        public static SearchUpdateResponse None => new SearchUpdateResponse(null, null, false, false);

        public UpdatePackage? SourcePackage { get; }
        public UpdatePackage? RecommendedPackage { get; }
        public bool IsEnforced { get; }
        public bool IsRollback { get; }
    }
}
