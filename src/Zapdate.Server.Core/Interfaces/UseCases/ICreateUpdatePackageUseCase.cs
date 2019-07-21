using Zapdate.Server.Core.Dto.UseCaseRequests;
using Zapdate.Server.Core.Dto.UseCaseResponses;

namespace Zapdate.Server.Core.Interfaces.UseCases
{
    public interface ICreateUpdatePackageUseCase : IUseCaseRequestHandler<CreateUpdatePackageRequest, CreateUpdatePackageResponse>
    {
    }
}
