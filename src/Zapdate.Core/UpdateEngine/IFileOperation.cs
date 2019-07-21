namespace Zapdate.Core.UpdateEngine
{
    /// <summary>
    ///     Defines a simple file operation
    /// </summary>
    public interface IFileOperation
    {
        /// <summary>
        ///     The type of the file operation
        /// </summary>
        FileOperationType OperationType { get; }
    }
}
