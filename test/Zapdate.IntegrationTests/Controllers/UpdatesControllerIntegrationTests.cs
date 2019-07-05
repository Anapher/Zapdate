using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Dto.Universal;
using Zapdate.IntegrationTests.Seeds;
using Zapdate.Models.Universal;
using UpdateFileDto = Zapdate.Models.Universal.UpdateFileDto;

namespace Zapdate.IntegrationTests.Controllers
{
    public class UpdatesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<ProjectSeed>>
    {
        private readonly HttpClient _client;

        public UpdatesControllerIntegrationTests(CustomWebApplicationFactory<ProjectSeed> factory)
        {
            _client = factory.CreateClient();
        }

        #region Create Update
        [Fact]
        public async Task CantCreateUpdateUnauthenticated()
        {
            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updates",
                new UpdatePackageDto
                {
                    Version = "1.0.0",
                    Files = new List<UpdateFileDto>
                { new UpdateFileDto { Path = "asd", Hash = "FF" } }
                });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CanCreateValidUpdatePackage()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updates",
                new UpdatePackageDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogInfo>
                    {
                        new UpdateChangelogInfo("de-de", "Neue Änderungen"),
                        new UpdateChangelogInfo("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("Test", DateTimeOffset.UtcNow) },
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CantCreatUpdatePackageWithNonExistingProject()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/2/updates",
                new UpdatePackageDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogInfo>
                    {
                        new UpdateChangelogInfo("de-de", "Neue Änderungen"),
                        new UpdateChangelogInfo("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("Test", DateTimeOffset.UtcNow) },
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CantCreatUpdatePackageWithNonExistingStoredFile()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updates",
                new UpdatePackageDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogInfo>
                    {
                        new UpdateChangelogInfo("de-de", "Neue Änderungen"),
                        new UpdateChangelogInfo("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("Test", DateTimeOffset.UtcNow) },
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d932" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CantCreateUpdatePackageWithInvalidVersion()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updates",
                new UpdatePackageDto
                {
                    Version = "asd",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogInfo>
                    {
                        new UpdateChangelogInfo("de-de", "Neue Änderungen"),
                        new UpdateChangelogInfo("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionInfo> { new UpdatePackageDistributionInfo("Test", DateTimeOffset.UtcNow) },
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        #endregion

        #region Patch Update
        [Fact]
        public async Task CantPatchUpdateUnauthenticated()
        {
            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Replace(x => x.Description, "New Description");

            var response = await _client.PatchAsync("/api/v1/projects/1/updates/1.5.0", patchDoc.AsJson());

            var asd = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CanPatchUpdatePackageDescription()
        {
            await TestUtils.Auth(_client);

            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Replace(x => x.Description, "New Description");

            var response = await _client.PatchAsync("/api/v1/projects/1/updates/1.5.0", patchDoc.AsJson());

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion
    }
}
