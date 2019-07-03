using CodeElements.Core;

namespace Zapdate.Core.Interfaces.Services
{
    public interface IAsymmetricCryptoHandler
    {
        bool VerifyHash(Hash hash, string signature, string publicKey);
        string SignHash(Hash hash, string privateKey);
    }
}
