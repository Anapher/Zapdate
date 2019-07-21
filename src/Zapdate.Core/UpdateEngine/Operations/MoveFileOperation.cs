using Zapdate.Core.Files;

namespace Zapdate.Core.UpdateEngine.Operations
{
	/// <summary>
	///     Move a file to a specific location
	/// </summary>
	public class MoveFileOperation : IFileOperation
	{
        public MoveFileOperation(IFileLocation sourceFile, IFileLocation targetLocation)
        {
            SourceFile = sourceFile;
            TargetLocation = targetLocation;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private MoveFileOperation()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        ///     The file from the source file base
        /// </summary>
        public IFileLocation SourceFile { get; private set; }

		/// <summary>
		///     The target location where the file should be moved to
		/// </summary>
		public IFileLocation TargetLocation { get; private set; }

		/// <summary>
		///     <see cref="FileOperationType.Move" />
		/// </summary>
		public FileOperationType OperationType { get; } = FileOperationType.Move;
	}
}