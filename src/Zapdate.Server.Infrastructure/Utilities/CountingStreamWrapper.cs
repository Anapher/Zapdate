using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Zapdate.Server.Infrastructure.Utilities
{
    internal class CountingStreamWrapper : Stream
    {
        private readonly Stream _baseStream;

        public CountingStreamWrapper(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        public long TotalDataRead { get; private set; }
        public long LastDataRead { get; private set; }

        public long TotalDataWritten { get; private set; }
        public long LastDataWritten { get; private set; }

        public override bool CanRead => _baseStream.CanRead;
        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;
        public override long Length => _baseStream.Length;

        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var read = _baseStream.Read(buffer, offset, count);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override int Read(Span<byte> buffer)
        {
            var read = _baseStream.Read(buffer);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            var read = await _baseStream.ReadAsync(buffer, cancellationToken);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var read = await _baseStream.ReadAsync(buffer, offset, count, cancellationToken);
            TotalDataRead += read;
            LastDataRead = read;
            return read;
        }

        public override int ReadByte()
        {
            var result = _baseStream.ReadByte();

            if (result == -1)
            {
                LastDataRead = 0;
            }
            else
            {
                LastDataRead = 1;
                TotalDataRead++;
            }

            return result;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _baseStream.Write(buffer, offset, count);
            LastDataWritten = count;
            TotalDataWritten += count;
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            LastDataWritten = count;
            TotalDataWritten += count;
            return _baseStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            _baseStream.Write(buffer);
            LastDataWritten = buffer.Length;
            TotalDataWritten += buffer.Length;
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            LastDataWritten = buffer.Length;
            TotalDataWritten += buffer.Length;
            return _baseStream.WriteAsync(buffer, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            _baseStream.WriteByte(value);
            LastDataWritten = 1;
            TotalDataWritten++;
        }
    }
}
