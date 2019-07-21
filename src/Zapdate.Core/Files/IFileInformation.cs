namespace Zapdate.Core.Files
{
    /// <summary>
    ///     Information about a file
    /// </summary>
    public interface IFileInformation : IFileLocation
    {
        /// <summary>
        ///     The file hash
        /// </summary>
        Hash Hash { get; }

        /// <summary>
        ///     The file length
        /// </summary>
        long Length { get; }
    }
}
