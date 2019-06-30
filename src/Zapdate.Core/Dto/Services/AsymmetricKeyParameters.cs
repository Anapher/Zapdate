namespace Zapdate.Core.Dto.Services
{
    public struct AsymmetricKeyParameters
    {
        public AsymmetricKeyParameters(string publicKey, string privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string PublicKey { get; }
        public string PrivateKey { get; }
    }
}
