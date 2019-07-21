namespace Zapdate.Server.Core.Dto.UseCaseResponses
{
    public class CreateUpdatePackageResponse
    {
        public CreateUpdatePackageResponse(int updatePackageId)
        {
            UpdatePackageId = updatePackageId;
        }

        public int UpdatePackageId { get; }
    }
}
