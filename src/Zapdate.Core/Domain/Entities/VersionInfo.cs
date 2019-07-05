using Zapdate.Core.Extensions;

namespace Zapdate.Core.Domain.Entities
{
    // value object
    public class VersionInfo
    {
        public VersionInfo(SemVersion version)
        {
            Version = version.ToString(false);
            Prerelease = version.Prerelease;
            Build = version.Build;

            BinaryVersion = version.ToBinaryVersion();
        }

#pragma warning disable CS8618 // Constructor for mapping
        private VersionInfo()
        {
        }
#pragma warning restore CS8618

        public string Version { get; private set; }
        public string? Prerelease { get; private set; }
        public string? Build { get; private set; }
        public long BinaryVersion { get; private set; }

        public SemVersion SemVersion
        {
            get => SemVersion.Parse(Version).Change(null, null, null, null, Build);
        }
    }
}
