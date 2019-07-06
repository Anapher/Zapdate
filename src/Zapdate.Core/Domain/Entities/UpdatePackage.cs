using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    // aggregate root
    public class UpdatePackage : BaseEntity
    {
        private HashSet<UpdatePackageDistribution> _distributions;
        private HashSet<UpdateChangelog> _changelogs;
        private HashSet<UpdateFile> _files;

#pragma warning disable CS8618 // Constructor for mapping
        private UpdatePackage()
        {
        }
#pragma warning restore CS8618

        public UpdatePackage(SemVersion version, int projectId = 0)
        {
            VersionInfo = new VersionInfo(version);
            OrderNumber = -1;

            CustomFields = ImmutableDictionary<string, string>.Empty;

            _distributions = new HashSet<UpdatePackageDistribution>();
            _changelogs = new HashSet<UpdateChangelog>();
            _files = new HashSet<UpdateFile>();

            ProjectId = projectId;
        }

        public int ProjectId { get; private set; }
        public string? Description { get; set; }
        public IImmutableDictionary<string, string> CustomFields { get; set; }
        public VersionInfo VersionInfo { get; protected set; }
        public int OrderNumber { get; set; }

        public bool IsListed => OrderNumber >= 0;

        public IEnumerable<UpdateChangelog> Changelogs => _changelogs;
        public IEnumerable<UpdatePackageDistribution> Distributions => _distributions;
        public IEnumerable<UpdateFile> Files => _files;

        public void UpdateVersion(SemVersion version)
        {
            VersionInfo = new VersionInfo(version);
            OrderNumber = -1;
        }

        public UpdatePackageDistribution AddDistribution(string name)
        {
            if (Distributions.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"The update package already has the channel {name}", nameof(name));

            var channel = new UpdatePackageDistribution(name, Id);
            _distributions.Add(channel);

            return channel;
        }

        public void RemoveDistribution(UpdatePackageDistribution channel)
        {
            _distributions.Remove(channel);
        }

        public void AddChangelog(string language, string content)
        {
            var changelog = Changelogs.FirstOrDefault(x => x.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (changelog != null)
                throw new ArgumentException($"A changelog with the language {language} does already exist.");

            // throws CultureNotFoundException if the culture does not exist
            CultureInfo.GetCultureInfo(language);

            _changelogs.Add(new UpdateChangelog(language, content, Id));
        }

        public void RemoveChangelog(string language)
        {
            var changelog = Changelogs.FirstOrDefault(x => x.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (changelog == null)
                throw new ArgumentException($"A changelog with the language {language} does not exist.");

            _changelogs.Remove(changelog);
        }

        public void AddFile(UpdateFile file)
        {
            if (_files.Any(x => x.Path.Equals(file.Path)))
                throw new ArgumentException($"A file with path {file.Path} already exists.");

            _files.Add(file);
        }

        public void RemoveFile(UpdateFile file)
        {
            _files.Remove(file);
        }
    }
}
