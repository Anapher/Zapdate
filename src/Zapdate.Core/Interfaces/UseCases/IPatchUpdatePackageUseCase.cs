﻿using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Core.Dto.UseCaseResponses;

namespace Zapdate.Core.Interfaces.UseCases
{
    public interface IPatchUpdatePackageUseCase : IUseCaseRequestHandler<PatchUpdatePackageRequest, PatchUpdatePackageResponse>
    {
    }
}