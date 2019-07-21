using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Server.Models.Request;

namespace Zapdate.Server.IntegrationTests
{
    public static class TestUtils
    {
        public static async Task Auth(HttpClient client)
        {
            var httpResponse = await client.PostAsync("/api/v1/auth/login",
                new StringContent(JsonConvert.SerializeObject(new LoginRequestDto { UserName = "mmacneil", Password = "Pa$$W0rd1" }),
                Encoding.UTF8, "application/json"));

            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(stringResponse);
            Assert.NotNull(result.accessToken);

            string token = result.accessToken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static StringContent AsJson(this object o)
            => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");
    }
}
