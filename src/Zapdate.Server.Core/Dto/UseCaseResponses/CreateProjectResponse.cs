namespace Zapdate.Server.Core.Dto.UseCaseResponses
{
    public class CreateProjectResponse
    {
        public CreateProjectResponse(int projectId, string? asymmetricKey = null)
        {
            ProjectId = projectId;
            AsymmetricKey = asymmetricKey;
        }

        public int ProjectId { get; }
        public string? AsymmetricKey { get; }
    }
}
