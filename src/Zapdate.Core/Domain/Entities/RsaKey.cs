using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    public class RsaKey : BaseEntity
    {
        public RsaKey(int projectId, string publicKey, string privateKey, bool isEncrypted)
        {
            ProjectId = projectId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
            IsPrivateKeyEncrypted = isEncrypted;
        }

#pragma warning disable CS8618 // Constructor for mapping
        public RsaKey()
        {
        }
#pragma warning restore CS8618

        public int ProjectId { get; private set; }

        public string PublicKey { get; private set; }

        public string? PrivateKey { get; private set; } // if null, only the user has the key
        public bool IsPrivateKeyEncrypted { get; private set; }

        public void SetPrivateKey(string? privateKey = null, bool isEncrypted = false)
        {
            PrivateKey = privateKey;
            IsPrivateKeyEncrypted = isEncrypted;
        }
    }
}
