using System;
using System.Security.Cryptography;
using Zapdate.Core.Domain;
using Zapdate.Core.Interfaces.Services;

namespace Zapdate.Infrastructure.Cryptography
{
    public class AsymmetricCryptoHandler : IAsymmetricCryptoHandler
    {
        private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;
        private readonly RSASignaturePadding _padding = RSASignaturePadding.Pss;

        public string SignHash(Hash hash, string privateKey)
        {
            if (!hash.IsSha256Size)
                throw new ArgumentException("SHA256 hashes are supported exclusively", nameof(hash));

            var parameters = AsymmetricKeyFactory.Deserialize(privateKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(parameters);

                var signature = rsa.SignHash(hash.HashData, _hashAlgorithm, _padding);
                return Convert.ToBase64String(signature);
            }
        }

        public bool VerifyHash(Hash hash, string signature, string publicKey)
        {
            if (!hash.IsSha256Size)
                throw new ArgumentException("SHA256 hashes are supported exclusivly", nameof(hash));

            var signatureData = Convert.FromBase64String(signature);
            var parameters = AsymmetricKeyFactory.Deserialize(publicKey);
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(parameters);

                return rsa.VerifyHash(hash.HashData, signatureData, _hashAlgorithm, _padding);
            }
        }
    }
}
