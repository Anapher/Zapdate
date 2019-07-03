using CodeElements.Core;
using System;
using System.Collections.Generic;
using Zapdate.Core.Domain;
using Zapdate.Core.Dto.UseCaseResponses;
using Zapdate.Core.Interfaces;

namespace Zapdate.Core.Dto.UseCaseRequests
{
    public class CreateUpdatePackageRequest : IUseCaseRequest<CreateUpdatePackageResponse>
    {
        public CreateUpdatePackageRequest(int projectId, SemVersion version, string? description, IDictionary<string, string>? customFields, IReadOnlyList<UpdateFileDto> files, IReadOnlyList<UpdateChangelogDto>? changelogs, IReadOnlyList<UpdatePackageDistributionDto>? distribution)
        {
            ProjectId = projectId;
            Version = version;
            Description = description;
            CustomFields = customFields ?? new Dictionary<string, string>();
            Files = files;
            Changelogs = changelogs;
            Distributions = distribution;
        }

        public string? AsymmetricKeyPassword { get; set; }
        public int ProjectId { get; }

        public SemVersion Version { get; }
        public string? Description { get; }
        public IDictionary<string, string> CustomFields { get; }

        public IReadOnlyList<UpdateFileDto> Files { get; set; }
        public IReadOnlyList<UpdateChangelogDto>? Changelogs { get; set; }
        public IReadOnlyList<UpdatePackageDistributionDto>? Distributions { get; set; }
    }

    public class UpdatePackageDistributionDto
    {
        public UpdatePackageDistributionDto(string name, DateTimeOffset? publishDate)
        {
            Name = name;
            PublishDate = publishDate;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdatePackageDistributionDto()
        {
        }
#pragma warning restore CS8618

        public string Name { get; private set; }
        public DateTimeOffset? PublishDate { get; private set; }
    }

    public class UpdateChangelogDto
    {
        public UpdateChangelogDto(string language, string content)
        {
            Language = language;
            Content = content;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateChangelogDto()
        {
        }
#pragma warning restore CS8618

        public string Language { get; set; }
        public string Content { get; set; }
    }

    public class UpdateFileDto
    {
        public UpdateFileDto(string path, Hash hash, string? signature = null)
        {
            Path = path;
            Hash = hash;
            Signature = signature;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateFileDto()
        {
        }
#pragma warning restore CS8618

        public string Path { get; private set; }
        public Hash Hash { get; private set; }
        public string? Signature { get; private set; }
    }
}
