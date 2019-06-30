using System.Text;
using Xunit;
using Zapdate.Infrastructure.Utilities;

namespace Zapdate.Infrastructure.Tests.Utilities
{
    public class UrlBase64Tests
    {
        [Theory]
        [InlineData("sadefadsfsdfsfddasipoäipoäipä")]
        [InlineData("*?=)(NÜ V§QW$%U`(M§QB+")]
        [InlineData("ÄÖLÜ?K*W?=U*JWAR")]
        [InlineData("-Ͽ")]
        public void TestEncode(string testData)
        {
            var data = UrlBase64.Encode(Encoding.UTF8.GetBytes(testData));

            Assert.DoesNotContain("/", data);
            Assert.DoesNotContain("=", data);
            Assert.DoesNotContain("+", data);

            var result = Encoding.UTF8.GetString(UrlBase64.Decode(data));
            Assert.Equal(testData, result);
        }
    }
}
