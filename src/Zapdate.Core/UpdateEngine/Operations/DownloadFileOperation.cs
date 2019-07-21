using Zapdate.Core.Files;

namespace Zapdate.Core.UpdateEngine.Operations
{
    /// <summary>
    ///     Download a new file
    /// </summary>
    public class DownloadFileOperation : IFileOperation
    {
        public DownloadFileOperation(IFileInformation target)
        {
            Target = target;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private DownloadFileOperation()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        ///     The location of the file
        /// </summary>
        public IFileInformation Target { get; private set; }

        /// <summary>
        ///     <see cref="FileOperationType.Download" />
        /// </summary>
        public FileOperationType OperationType { get; } = FileOperationType.Download;
    }
}
