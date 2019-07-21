namespace Zapdate.Server.Core.Dto.Universal
{
    public class UpdateChangelogInfo
    {
        public UpdateChangelogInfo(string language, string content)
        {
            Language = language;
            Content = content;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateChangelogInfo()
        {
        }
#pragma warning restore CS8618

        public string Language { get; set; }
        public string Content { get; set; }
    }
}
