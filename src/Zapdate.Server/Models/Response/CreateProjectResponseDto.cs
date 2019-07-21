namespace Zapdate.Server.Models.Response
{
    public class CreateProjectResponseDto
    {
        public CreateProjectResponseDto(int projectId, string? asymmetricKey = null)
        {
            ProjectId = projectId;
            AsymmetricKey = asymmetricKey;
        }

        public int ProjectId { get; }
        public string? AsymmetricKey { get; }
    }
}
