using Zapdate.Core;

namespace Zapdate.Server.Core.Extensions
{
    public static class SemVersionExtensions
    {
        /// <summary>
        ///     Converts the major and minor part of the version to a 64 integer that can be sorted
        /// </summary>
        public static long ToBinaryVersion(this SemVersion version)
        {
            return ((long) version.Major << 32) | (uint) version.Minor;
        }
    }
}
