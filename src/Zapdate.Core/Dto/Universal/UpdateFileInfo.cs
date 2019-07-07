using Zapdate.Core.Domain;

namespace Zapdate.Core.Dto.Universal
{
    public class UpdateFileInfo
    {
        public UpdateFileInfo(string path, Hash hash, string? signature = null)
        {
            Path = path;
            Hash = hash;
            Signature = signature;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateFileInfo()
        {
        }
#pragma warning restore CS8618

        public string Path { get; private set; }
        public Hash Hash { get; private set; }
        public string? Signature { get; private set; }
    }
}
