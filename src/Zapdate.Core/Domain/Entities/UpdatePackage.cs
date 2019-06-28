using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapdate.Core.Shared;

namespace Zapdate.Core.Domain.Entities
{
    public class UpdatePackage : BaseEntity
    {
        private readonly IList<UpdateChannel> _channels;
        private readonly IList<UpdateChangelog> _changelogs;

#pragma warning disable CS8618 // Constructor for mapping
        public UpdatePackage()
        {
        }
#pragma warning restore CS8618

        public UpdatePackage(SemVersion version)
        {
            Version = version;
            CustomFields = new Dictionary<string, string>();

            _channels = new List<UpdateChannel>();
            _changelogs = new List<UpdateChangelog>();
        }

        public SemVersion Version { get; private set; }
        public string? Description { get; set; }
        public IDictionary<string, string> CustomFields { get; private set; }

        public IEnumerable<UpdateChangelog>? Changelogs { get; }
        public IEnumerable<UpdateChannel>? Channels => _channels;

        public UpdateChannel AddChannel(string name)
        {
            if (Channels.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new ArgumentException($"The update package already has the channel {name}", nameof(name));

            var channel = new UpdateChannel(Id, name);
            _channels.Add(channel);

            return channel;
        }

        public void RemoveChannel(UpdateChannel channel)
        {
            _channels.Remove(channel);
        }

        public void AddChangelog(string language, string content)
        {
            var changelog = Changelogs.FirstOrDefault(x => x.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (changelog != null)
                throw new ArgumentException($"A changelog with the language {language} does already exist.");


            _changelogs.Add(new UpdateChangelog(Id, language, content));
        }

        public void RemoveChangelog(string language)
        {
            var changelog = Changelogs.FirstOrDefault(x => x.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (changelog == null)
                throw new ArgumentException($"A changelog with the language {language} does not exist.");

            _changelogs.Remove(changelog);
        }
    }
}
