using Zapdate.Core.Files;

namespace Zapdate.Core.UpdateEngine.Operations
{
    /// <summary>
    ///     Delete a file
    /// </summary>
    public class DeleteFileOperation : IFileOperation
    {
        public DeleteFileOperation(IFileLocation target)
        {
            Target = target;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private DeleteFileOperation()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        ///     The target file that should be deleted
        /// </summary>
        public IFileLocation Target { get; private set; }

        /// <summary>
        ///     <see cref="FileOperationType.Delete" />
        /// </summary>
        public FileOperationType OperationType { get; } = FileOperationType.Delete;
    }
}
