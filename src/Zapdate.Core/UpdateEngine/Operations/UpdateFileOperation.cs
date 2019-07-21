using Zapdate.Core.Files;

namespace Zapdate.Core.UpdateEngine.Operations
{
    /// <summary>
    ///     Update an existing file
    /// </summary>
    public class UpdateFileOperation : IFileOperation
    {
        public UpdateFileOperation(IFileInformation target)
        {
            Target = target;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private UpdateFileOperation()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        ///     The location of the file
        /// </summary>
        public IFileInformation Target { get; private set; }

        /// <summary>
        ///     <see cref="FileOperationType.Update" />
        /// </summary>
        public FileOperationType OperationType { get; } = FileOperationType.Update;
    }
}
