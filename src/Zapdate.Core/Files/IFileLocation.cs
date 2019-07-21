namespace Zapdate.Core.Files
{
    /// <summary>
    ///     The location of a file
    /// </summary>
    public interface IFileLocation
    {
        /// <summary>
        ///     The file name (including path)
        /// </summary>
        string Filename { get; }
    }
}
