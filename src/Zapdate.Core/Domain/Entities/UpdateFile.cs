namespace Zapdate.Core.Domain.Entities
{
    public class UpdateFile
    {
        public UpdateFile(string path, string hash, string signature)
        {
            Path = path;
            Hash = hash;
            Signature = signature;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateFile()
        {
        }
#pragma warning restore CS8618


        public int Id { get; private set; }
        public int UpdatePackageId { get; private set; }

        public string Path { get; private set; }
        public string Hash { get; private set; }
        public string Signature { get; private set; }
    }
}
