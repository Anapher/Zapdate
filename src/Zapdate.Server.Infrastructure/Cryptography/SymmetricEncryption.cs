using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Zapdate.Server.Core.Interfaces.Services;
using Zapdate.Server.Infrastructure.Utilities;

namespace Zapdate.Server.Infrastructure.Cryptography
{
    public class SymmetricEncryption : ISymmetricEncryption
    {
        private const int KeySize = 256;
        private const int DerivationIterations = 1000;
        private readonly Encoding _encoding = Encoding.UTF8;

        private void Encrypt(byte[] data, byte[] passPhrase, Stream targetStream)
        {
            var saltBytes = Generate256BitsOfRandomEntropy();

            using (var aes = Aes.Create())
            {
                aes.GenerateIV();
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = KeySize;

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltBytes, DerivationIterations))
                    aes.Key = password.GetBytes(KeySize / 8);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    targetStream.Write(saltBytes, 0, saltBytes.Length);
                    targetStream.Write(BitConverter.GetBytes(aes.IV.Length), 0, 4);
                    targetStream.Write(aes.IV, 0, aes.IV.Length);

                    using (var cryptoStream = new CryptoStream(targetStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(BitConverter.GetBytes(data.Length), 0, 4);
                        cryptoStream.Write(data, 0, data.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                }
            }
        }

        public byte[] Decrypt(Stream dataStream, byte[] passPhrase)
        {
            var binaryReader = new BinaryReader(dataStream);
            var saltBytes = binaryReader.ReadBytes(32);
            var iv = binaryReader.ReadBytes(binaryReader.ReadInt32());

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = KeySize;

                using (var password = new Rfc2898DeriveBytes(passPhrase, saltBytes, DerivationIterations))
                    aes.Key = password.GetBytes(KeySize / 8);

                using (var decryptor = aes.CreateDecryptor(aes.Key, iv))
                {
                    using (var cryptoStream = new CryptoStream(dataStream, decryptor, CryptoStreamMode.Read))
                    {
                        var buffer = new byte[4];
                        cryptoStream.Read(buffer, 0, 4);
                        buffer = new byte[BitConverter.ToInt32(buffer, 0)];

                        var read = cryptoStream.Read(buffer, 0, buffer.Length);
                        if (read != buffer.Length)
                            throw new InvalidOperationException("The expected data length does not match the encrypted data.");

                        return buffer;
                    }
                }
            }
        }

        public byte[] Encrypt(byte[] data, byte[] passPhrase)
        {
            using (var memoryStream = new MemoryStream())
            {
                Encrypt(data, passPhrase, memoryStream);
                return memoryStream.ToArray();
            }
        }

        public byte[] Decrypt(byte[] source, byte[] passPhrase)
        {
            using (var memoryStream = new MemoryStream(source, false))
            {
                return Decrypt(memoryStream, passPhrase);
            }
        }

        public string DecryptString(string cipherText, string passPhrase)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            if (string.IsNullOrEmpty(passPhrase))
                throw new ArgumentNullException(nameof(passPhrase));

            var data = UrlBase64.Decode(cipherText);
            var plainBytes = Decrypt(data, _encoding.GetBytes(passPhrase));

            return _encoding.GetString(plainBytes);
        }

        public string EncryptString(string plainText, string passPhrase)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));

            if (string.IsNullOrEmpty(passPhrase))
                throw new ArgumentNullException(nameof(passPhrase));

            var data = _encoding.GetBytes(plainText);
            var key = _encoding.GetBytes(passPhrase);
            var encrypted = Encrypt(data, key);

            return UrlBase64.Encode(encrypted);
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
