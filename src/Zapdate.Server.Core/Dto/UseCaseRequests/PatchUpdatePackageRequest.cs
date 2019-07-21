using Zapdate.Core;
using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Dto.UseCaseRequests
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
