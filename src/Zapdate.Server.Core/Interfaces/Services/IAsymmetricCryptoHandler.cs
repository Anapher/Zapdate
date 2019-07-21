using Zapdate.Core;

namespace Zapdate.Server.Core.Interfaces.Services
{
    public interface IAsymmetricCryptoHandler
    {
        bool VerifyHash(Hash hash, string signature, string publicKey);
        string SignHash(Hash hash, string privateKey);
    }
}
