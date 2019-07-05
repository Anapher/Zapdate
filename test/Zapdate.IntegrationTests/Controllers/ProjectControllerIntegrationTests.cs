using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.IntegrationTests.Seeds;
using Zapdate.Models.Request;

namespace Zapdate.IntegrationTests.Controllers
{
    public class ProjectControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<AccountSeed>>
    {
        private readonly HttpClient _client;

        public ProjectControllerIntegrationTests(CustomWebApplicationFactory<AccountSeed> factory)
        {
            _client = factory.CreateClient();
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
            await TestUtils.Auth(_client);

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
            await TestUtils.Auth(_client);

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
            await TestUtils.Auth(_client);

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
