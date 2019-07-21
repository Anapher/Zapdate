using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.Core.Dto.UseCaseResponses;
using Zapdate.Server.Core.Interfaces;

namespace Zapdate.Server.Core.Dto.UseCaseRequests
{
    public class CreateUpdatePackageRequest : IUseCaseRequest<CreateUpdatePackageResponse>
    {
        public CreateUpdatePackageRequest(int projectId, UpdatePackageInfo updatePackage, string? asymmetricKeyPassword = null)
        {
            ProjectId = projectId;
            UpdatePackage = updatePackage;
            AsymmetricKeyPassword = asymmetricKeyPassword;
        }

        public string? AsymmetricKeyPassword { get; set; }
        public int ProjectId { get; }
        public UpdatePackageInfo UpdatePackage { get; }
    }
}
