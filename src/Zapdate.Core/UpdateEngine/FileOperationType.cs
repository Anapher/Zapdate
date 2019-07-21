namespace Zapdate.Core.UpdateEngine
{
    // Ordered by importance. First, the files must be moved (before they are updated).
    // Then, files have to be deleted and updated (that shouldnt interfere)

    /// <summary>
    ///     File operation types
    /// </summary>
    public enum FileOperationType
    {
        /// <summary>
        ///     Copy a file
        /// </summary>
        Copy,

        /// <summary>
        ///     Move a file
        /// </summary>
        Move,

        /// <summary>
        ///     Delete a file
        /// </summary>
        Delete,

        /// <summary>
        ///     Update an existing file
        /// </summary>
        Update,

        /// <summary>
        ///     Download a new file
        /// </summary>
        Download,

        /// <summary>
        ///     Update an existing file using delta patches
        /// </summary>
        DeltaPatch
    }
}
