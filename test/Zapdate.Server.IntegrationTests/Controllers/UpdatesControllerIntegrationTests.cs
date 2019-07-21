using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core;
using Zapdate.Server.Core.Dto.Universal;
using Zapdate.Server.IntegrationTests.Seeds;
using Zapdate.Server.Models.Universal;
using UpdateFileDto = Zapdate.Server.Models.Universal.UpdateFileDto;

namespace Zapdate.Server.IntegrationTests.Controllers
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

            var package = await FetchUpdatePackage(1, "1.5.0");
            Assert.Equal("New Description", package.Description);
        }

        [Fact]
        public async Task CanPatchUpdatePackageChangelog()
        {
            await TestUtils.Auth(_client);

            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Replace(x => x.Changelogs[0], new UpdateChangelogInfo("de-de", "Hello World 2.0"));

            // act
            var response = await _client.PatchAsync("/api/v1/projects/1/updates/3.0.0", patchDoc.AsJson());

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var package = await FetchUpdatePackage(1, "3.0.0");
            Assert.Equal("Hello World 2.0", package.Changelogs[0].Content);
        }

        [Fact]
        public async Task CanPatchUpdatePackageRemoveChangelog()
        {
            await TestUtils.Auth(_client);

            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Remove(x => x.Changelogs[0]);

            // act
            var response = await _client.PatchAsync("/api/v1/projects/1/updates/2.0.0", patchDoc.AsJson());

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var package = await FetchUpdatePackage(1, "2.0.0");
            Assert.Empty(package.Changelogs);
        }

        [Fact]
        public async Task CanPatchUpdatePackagePublish()
        {
            await TestUtils.Auth(_client);

            var publishDate = DateTimeOffset.UtcNow.AddHours(-1);
            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Add(x => x.Distribution, new UpdatePackageDistributionInfo("team", publishDate));

            // act
            var response = await _client.PatchAsync("/api/v1/projects/1/updates/2.0.0", patchDoc.AsJson());

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var package = await FetchUpdatePackage(1, "2.0.0");
            var dist = Assert.Single(package.Distribution);
            Assert.Equal("team", dist.Name);
            Assert.Equal(publishDate, dist.PublishDate);
        }

        [Fact]
        public async Task CanPatchUpdatePackageAddFile()
        {
            await TestUtils.Auth(_client);

            var patchDoc = new JsonPatchDocument<UpdatePackageDto>();
            patchDoc.Add(x => x.Files, new UpdateFileDto {Path = "/new-file.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" });

            // act
            var response = await _client.PatchAsync("/api/v1/projects/1/updates/2.0.0", patchDoc.AsJson());

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var package = await FetchUpdatePackage(1, "2.0.0");
            Assert.Collection(package.Files.OrderBy(x => x.Path),
                x =>
                {
                    Assert.Equal("/new-file.txt", x.Path);
                    Assert.Equal("936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af", x.Hash);
                    Assert.NotNull(x.Signature);
                },
                x => Assert.Equal("/test.txt", x.Path));
        }
        #endregion

        public async Task<UpdatePackageDto> FetchUpdatePackage(int projectId, SemVersion version)
        {
            var json = await _client.GetStringAsync($"/api/v1/projects/{projectId}/updates/{version}");
            return JsonConvert.DeserializeObject<UpdatePackageDto>(json);
        }
    }
}
