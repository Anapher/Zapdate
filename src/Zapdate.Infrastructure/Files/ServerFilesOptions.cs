namespace Zapdate.Infrastructure.Files
{
    public class ServerFilesOptions
    {
        /// <summary>
        ///     The directory where the files should be stored
        /// </summary>
        public string Directory { get; set; } = "files";

        /// <summary>
        ///     The temporary directory used to cache files
        /// </summary>
        public string TempDirectory { get; set; } = "temp";
    }
}
