namespace Zapdate.Server.Core.Domain.Entities
{
    // aggregate root
    /// <summary>
    ///     A file stored on the server, independ of any update package (to prevent duplication)
    ///     The file is compressed
    /// </summary>
    public class StoredFile
    {
        public StoredFile(string fileHash, long length, long compressedLength)
        {
            FileHash = fileHash;
            Length = length;
            CompressedLength = compressedLength;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private StoredFile()
        {
        }
#pragma warning restore CS8618


        /// <summary>
        ///     The actual, uncompressed file hash. This is also the identity of this entity
        /// </summary>
        public string FileHash { get; private set; }

        /// <summary>
        ///     The length of the actual, uncompressed file.
        /// </summary>
        public long Length { get; private set; }

        /// <summary>
        ///     The length of the compressed file, as it is on the server
        /// </summary>
        public long CompressedLength { get; private set; }
    }
}
