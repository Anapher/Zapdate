using Zapdate.Core.Domain;

namespace Zapdate.Core.Interfaces.Services
{
    public interface IAsymmetricCryptoHandler
    {
        bool VerifyHash(Hash hash, string signature, string publicKey);
        string SignHash(Hash hash, string privateKey);
    }
}
