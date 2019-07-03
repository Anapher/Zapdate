using Xunit;
using Zapdate.Infrastructure.Cryptography;

namespace Zapdate.Infrastructure.Tests.Cryptography
{
    public class AsymmetricKeyFactoryTests
    {
        [Fact]
        public void TestCreateKey()
        {
            var factory = new AsymmetricKeyFactory();
            var key = factory.Create();

            Assert.NotNull(key.PrivateKey);
            Assert.NotNull(key.PublicKey);

            Assert.DoesNotContain("\"dp\":", key.PublicKey);
            Assert.Contains("\"dp\":", key.PrivateKey);

            var publicKeyParams = AsymmetricKeyFactory.Deserialize(key.PublicKey);
            Assert.Null(publicKeyParams.DQ);
            Assert.NotNull(publicKeyParams.Modulus);
            Assert.NotNull(publicKeyParams.Exponent);

            var privateKeyParams = AsymmetricKeyFactory.Deserialize(key.PrivateKey);
            Assert.NotNull(privateKeyParams.DQ);
            Assert.NotNull(privateKeyParams.Modulus);
        }
    }
}
