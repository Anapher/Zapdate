using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    public class AsymmetricKey
    {
        public AsymmetricKey(string publicKey, string? privateKey = null, bool isEncrypted = false, int projectId = 0)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
            IsPrivateKeyEncrypted = isEncrypted;
            ProjectId = projectId;
        }

#pragma warning disable CS8618 // Constructor for mapping
        private AsymmetricKey()
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
