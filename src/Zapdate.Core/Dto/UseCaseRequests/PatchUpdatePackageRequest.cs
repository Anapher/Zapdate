using Zapdate.Core.Domain;
using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core.Dto.UseCaseRequests
{
    public class PatchUpdatePackageRequest : IUseCaseRequest<PatchUpdatePackageResponse>
    {
        public PatchUpdatePackageRequest(int projectId, SemVersion currentVersion, UpdatePackageInfo updatePackage, string? asymmetricKeyPassword)
        {
            ProjectId = projectId;
            CurrentVersion = currentVersion;
            UpdatePackage = updatePackage;
            AsymmetricKeyPassword = asymmetricKeyPassword;
        }

        public int ProjectId { get; }
        public SemVersion CurrentVersion { get; }
        public UpdatePackageInfo UpdatePackage { get; }
        public string? AsymmetricKeyPassword { get; set; }
    }
}
