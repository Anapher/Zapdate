using Zapdate.Core.Dto.Universal;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core.Dto.UseCaseRequests
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
