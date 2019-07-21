using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xunit;
using Zapdate.Core;
using Zapdate.Server.Infrastructure.Files;

namespace Zapdate.Server.Infrastructure.Tests.Files
{
    public class ServerFilesManagerTests
    {
        private readonly byte[] _testFile1 = new byte[] { 29, 76, 89, 223, 92, 198, 48, 136, 5, 102, 40, 224, 218, 235, 51, 75, 92, 12, 96, 109 };
        private readonly Hash _testFile1Hash = Hash.Parse("67c98dce4c66fcc876a1350656efa8a1f3ee41ff6bd846b2c81703c29ad97c9c");

        [Fact]
        public async Task TestAddFileNothingExists()
        {
            var fs = new MockFileSystem();
            var options = new ServerFilesOptions { Directory = "D:\\files", TempDirectory = "D:\\temp" };

            var filesManager = new ServerFilesManager(fs, Options.Create(options), GetNullLogger());
            var testFileStream = new MemoryStream(_testFile1);

            // act
            var storedFile = await filesManager.AddFile(testFileStream);

            // assert
            Assert.Equal(_testFile1Hash.ToString(), storedFile.FileHash);
            Assert.Equal(_testFile1.Length, storedFile.Length);
            Assert.True(fs.FileExists($"{options.Directory}\\{_testFile1Hash}"));
            Assert.Empty(fs.Directory.GetFiles(options.TempDirectory));
        }

        [Fact]
        public async Task TestAddFileThatAlreadyExists()
        {
            var fs = new MockFileSystem();
            fs.AddFile($"D:\\files\\{_testFile1Hash}", new MockFileData(new byte[] { 1 }));

            var options = new ServerFilesOptions { Directory = "D:\\files", TempDirectory = "D:\\temp" };

            var filesManager = new ServerFilesManager(fs, Options.Create(options), GetNullLogger());
            var testFileStream = new MemoryStream(_testFile1);

            // act
            var storedFile = await filesManager.AddFile(testFileStream);

            // assert
            Assert.Equal(_testFile1Hash.ToString(), storedFile.FileHash);
            Assert.Equal(_testFile1.Length, storedFile.Length);
            Assert.True(fs.FileExists($"{options.Directory}\\{_testFile1Hash}"));
            Assert.Empty(fs.Directory.GetFiles(options.TempDirectory));
        }

        [Fact]
        public async Task TestReadFile()
        {
            var fs = new MockFileSystem();
            var options = new ServerFilesOptions { Directory = "D:\\files", TempDirectory = "D:\\temp" };

            var filesManager = new ServerFilesManager(fs, Options.Create(options), GetNullLogger());
            var testFileStream = new MemoryStream(_testFile1);
            await filesManager.AddFile(testFileStream);

            // act
            var file = filesManager.ReadFile(_testFile1Hash);

            Hash hash;
            using (var gzip = new GZipStream(file, CompressionMode.Decompress, false))
            {
                hash = new Hash(SHA256.Create().ComputeHash(gzip));
            }

            // assert
            Assert.Equal(_testFile1Hash.ToString(), hash.ToString());
        }

        private ILogger<ServerFilesManager> GetNullLogger() =>
            new Logger<ServerFilesManager>(NullLoggerFactory.Instance);
    }
}
