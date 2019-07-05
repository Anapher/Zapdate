using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core.Dto.UseCaseRequests;
using Zapdate.IntegrationTests.Seeds;
using Zapdate.Models.Request;
using UpdateFileDto = Zapdate.Models.Request.UpdateFileDto;

namespace Zapdate.IntegrationTests.Controllers
{
    public class UpdatePackagesControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<ProjectSeed>>
    {
        private readonly HttpClient _client;

        public UpdatePackagesControllerIntegrationTests(CustomWebApplicationFactory<ProjectSeed> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CantCreateProjectUnauthenticated()
        {
            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updatePackages", 
                new CreateUpdatePackageRequestDto { Version = "1.0.0", Files = new List<UpdateFileDto>
                { new UpdateFileDto { Path ="asd", Hash = "FF" } }  });

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CanCreateValidUpdatePackage()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updatePackages",
                new CreateUpdatePackageRequestDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogDto>
                    {
                        new UpdateChangelogDto("de-de", "Neue Änderungen"),
                        new UpdateChangelogDto("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true"} },
                    Distribution = new List<UpdatePackageDistributionDto> { new UpdatePackageDistributionDto("Test", DateTimeOffset.UtcNow)},
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CantCreatUpdatePackageWithNonExistingProject()
        {
            await TestUtils.Auth(_client);

            var response = await _client.PostAsJsonAsync("/api/v1/projects/2/updatePackages",
                new CreateUpdatePackageRequestDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogDto>
                    {
                        new UpdateChangelogDto("de-de", "Neue Änderungen"),
                        new UpdateChangelogDto("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionDto> { new UpdatePackageDistributionDto("Test", DateTimeOffset.UtcNow) },
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

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updatePackages",
                new CreateUpdatePackageRequestDto
                {
                    Version = "1.0.0",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogDto>
                    {
                        new UpdateChangelogDto("de-de", "Neue Änderungen"),
                        new UpdateChangelogDto("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionDto> { new UpdatePackageDistributionDto("Test", DateTimeOffset.UtcNow) },
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

            var response = await _client.PostAsJsonAsync("/api/v1/projects/1/updatePackages",
                new CreateUpdatePackageRequestDto
                {
                    Version = "asd",
                    Description = "Test update package",
                    Changelogs = new List<UpdateChangelogDto>
                    {
                        new UpdateChangelogDto("de-de", "Neue Änderungen"),
                        new UpdateChangelogDto("en-us", "New changes")
                    },
                    CustomFields = new Dictionary<string, string> { { "requireAdmin", "true" } },
                    Distribution = new List<UpdatePackageDistributionDto> { new UpdatePackageDistributionDto("Test", DateTimeOffset.UtcNow) },
                    Files = new List<UpdateFileDto>
                    {
                        new UpdateFileDto { Path = "/file1.txt", Hash = "13879d586271db46f545166cf67ef5aa585fe99b6121b7324467f9c751a8d93b" },
                        new UpdateFileDto { Path = "/file2.txt", Hash = "936a185caaa266bb9cbe981e9e05cb78cd732b0b3280eb944412bb6f8f8f07af" },
                    }
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
