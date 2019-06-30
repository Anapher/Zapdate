using Xunit;
using Zapdate.Infrastructure.Cryptography;

namespace Zapdate.Infrastructure.Tests.Cryptography
{
    public class SymmetricEncryptionTests
    {
        private SymmetricEncryption _encryption;

        public SymmetricEncryptionTests()
        {
            _encryption = new SymmetricEncryption();
        }

        [Fact]
        public void TestEncryptDecryptString()
        {
            var testString = "DAS IST DAS WORT!!!";
            var password = "MElINA";

            var encrypted = _encryption.EncryptString(testString, password);
            Assert.NotNull(encrypted);

            var decrypted = _encryption.DecryptString(encrypted, password);
            Assert.Equal(testString, decrypted);
        }
    }
}
