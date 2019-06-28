namespace Zapdate.Core.Domain.Entities
{
    public class UpdateChangelog
    {
        public UpdateChangelog(int updatePackageId, string language, string content)
        {
            UpdatePackageId = updatePackageId;
            Language = language;
            Content = content;
        }

#pragma warning disable CS8618 // Constructor for mapping
        public UpdateChangelog()
        {
        }
#pragma warning restore CS8618

        public int UpdatePackageId { get; private set; }
        public string Language { get; private set; }
        public string Content { get; set; }
    }
}
