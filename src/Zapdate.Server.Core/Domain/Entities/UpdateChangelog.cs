namespace Zapdate.Server.Core.Domain.Entities
{
    public class UpdateChangelog
    {
        public UpdateChangelog(string language, string content, int updatePackageId = 0)
        {
            Language = language;
            Content = content;
            UpdatePackageId = updatePackageId;
        }

#pragma warning disable CS8618 // Constructor for mapping
        private UpdateChangelog()
        {
        }
#pragma warning restore CS8618

        public int Id { get; private set; }
        public int UpdatePackageId { get; private set; }
        public string Language { get; private set; }
        public string Content { get; set; }
    }
}
