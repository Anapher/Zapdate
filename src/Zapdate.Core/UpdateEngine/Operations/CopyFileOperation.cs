using Zapdate.Core.Files;

namespace Zapdate.Core.UpdateEngine.Operations
{
	/// <summary>
	///     Copy a file to a specific location
	/// </summary>
	public class CopyFileOperation : IFileOperation
	{
        public CopyFileOperation(IFileLocation sourceFile, IFileLocation targetLocation)
        {
            SourceFile = sourceFile;
            TargetLocation = targetLocation;
        }

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        private CopyFileOperation()
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        /// <summary>
        ///     The file from the source file base
        /// </summary>
        public IFileLocation SourceFile { get; private set; }

		/// <summary>
        ///     The target location where the file should be copied to
		/// </summary>
		public IFileLocation TargetLocation { get; private set; }

		/// <summary>
		///     <see cref="FileOperationType.Copy" />
		/// </summary>
		public FileOperationType OperationType { get; } = FileOperationType.Copy;
	}
}