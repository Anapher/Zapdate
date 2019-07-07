using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Zapdate.Core.Domain;
using Zapdate.Core.Domain.Entities;
using Zapdate.Infrastructure.Interfaces;
using Zapdate.Infrastructure.Utilities;

namespace Zapdate.Infrastructure.Files
{
    public class ServerFilesManager : IServerFilesManager
    {
        private readonly IFileSystem _fileSystem;
        private readonly ServerFilesOptions _options;
        private readonly ILogger _logger;

        public ServerFilesManager(IFileSystem fileSystem, IOptions<ServerFilesOptions> options, ILogger<ServerFilesManager> logger)
        {
            _fileSystem = fileSystem;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<StoredFile> AddFile(Stream stream)
        {
            Hash fileHash;
            IFileInfo tempFile;
            long uncompressedLength;

            using (var tempFileStream = CreateTempFile(out tempFile))
            using (var tempFileGzipStream = new GZipStream(tempFileStream, CompressionLevel.Optimal))
            using (var hashAlg = SHA256.Create())
            using (var cryptoStream = new CryptoStream(tempFileGzipStream, hashAlg, CryptoStreamMode.Write))
            using (var countingStream = new CountingStreamWrapper(cryptoStream))
            {
                await stream.CopyToAsync(countingStream);

                cryptoStream.FlushFinalBlock();
                fileHash = new Hash(hashAlg.Hash);
                uncompressedLength = countingStream.TotalDataWritten;
            }

            _logger.LogDebug("Downloaded file with hash {fileHash}", fileHash);

            var destinationFile = GetFilePath(fileHash);
            if (!_fileSystem.File.Exists(destinationFile))
            {
                _fileSystem.Directory.CreateDirectory(_options.Directory);

                try
                {
                    tempFile.MoveTo(destinationFile);
                }
                catch (Exception e)
                {
                    _logger.LogDebug(e, "Error occurred when moving file {fileHash} ({path}) to {destination}.",
                        fileHash, tempFile.FullName, destinationFile);

                    if (_fileSystem.File.Exists(destinationFile))
                    {
                        _logger.LogDebug("The file {fileHash} does already exist (likely a race condition), delete temp file and continue.", fileHash);

                        tempFile.Delete();
                    }
                    else
                        throw;
                }
            }
            else
            {
                _logger.LogDebug("File {fileHash} does already exist, delete downloaded file.", fileHash);
                tempFile.Delete();
            }

            var destinationFileLength = _fileSystem.FileInfo.FromFileName(destinationFile).Length;
            return new StoredFile(fileHash.ToString(), uncompressedLength, destinationFileLength);
        }

        public Stream ReadFile(Hash fileHash)
        {
            var path = GetFilePath(fileHash);
            return _fileSystem.FileStream.Create(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        private string GetFilePath(Hash hash)
        {
            return _fileSystem.Path.Combine(_options.Directory, hash.ToString());
        }

        private Stream CreateTempFile(out IFileInfo fileInfo)
        {
            _fileSystem.Directory.CreateDirectory(_options.TempDirectory);

            while (true)
            {
                fileInfo = _fileSystem.FileInfo.FromFileName(_fileSystem.Path.Combine(_options.TempDirectory,
                    Guid.NewGuid().ToString("D")));

                // technically, this is nonsense, as the GUID is by design unique, but you know, just in case ;)
                if (fileInfo.Exists)
                    continue;

                Stream fileStream;
                try
                {
                    fileStream = _fileSystem.FileStream.Create(fileInfo.FullName, FileMode.CreateNew, FileAccess.ReadWrite);
                }
                catch (IOException)
                {
                    continue;
                }

                // to make the file existing
                fileInfo.Refresh();
                return fileStream;
            }
        }
    }
}
