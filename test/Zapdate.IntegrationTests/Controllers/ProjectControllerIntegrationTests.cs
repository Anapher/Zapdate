using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.Models.Request;

namespace Zapdate.IntegrationTests.Controllers
{
    public class ProjectControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        private async Task Auth()
        {
            var httpResponse = await _client.PostAsync("/api/v1/auth/login", new StringContent(JsonConvert.SerializeObject(new LoginRequestDto { UserName = "mmacneil", Password = "Pa$$W0rd1" }), Encoding.UTF8, "application/json"));
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(stringResponse);
            Assert.NotNull(result.accessToken);

            string token = result.accessToken;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        [Fact]
        public async Task CantCreateProjectUnauthenticated()
        {
            var response = await _client.PostAsJsonAsync("/api/v1/projects", new CreateProjectRequestDto { ProjectName = "Hello World", RsaKeyStorage = KeyStorage.Server });
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CanCreateProjectWithServerStorage()
        {
            await Auth();

            var response = await _client.PostAsJsonAsync("/api/v1/projects", new CreateProjectRequestDto { ProjectName = "Hello World", RsaKeyStorage = KeyStorage.Server });
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(stringResponse);

            string key = result.asymmetricKey;
            Assert.Null(key);

            Assert.NotNull(result.projectId);
        }

        [Fact]
        public async Task CanCreateProjectWithServerEncryptedStorage()
        {
            await Auth();

            var response = await _client.PostAsJsonAsync("/api/v1/projects", new CreateProjectRequestDto { ProjectName = "Hello World", RsaKeyStorage = KeyStorage.ServerEncrypted, RsaKeyPassword = "testpw" });
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(stringResponse);

            string key = result.asymmetricKey;
            Assert.Null(key);

            Assert.NotNull(result.projectId);
        }

        [Fact]
        public async Task CanCreateProjectWithLocallyStorage()
        {
            await Auth();

            var response = await _client.PostAsJsonAsync("/api/v1/projects", new CreateProjectRequestDto { ProjectName = "Hello World", RsaKeyStorage = KeyStorage.Locally });
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(stringResponse);

            string key = result.asymmetricKey;
            Assert.NotNull(key);

            Assert.NotNull(result.projectId);
        }
    }
}
