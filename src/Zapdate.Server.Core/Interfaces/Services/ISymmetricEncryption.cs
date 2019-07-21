namespace Zapdate.Server.Core.Interfaces.Services
{
    public interface ISymmetricEncryption
    {
        string EncryptString(string plainText, string passPhrase);
        string DecryptString(string cipherText, string passPhrase);

        byte[] Encrypt(byte[] data, byte[] passPhrase);
        byte[] Decrypt(byte[] data, byte[] passPhrase);
    }
}
