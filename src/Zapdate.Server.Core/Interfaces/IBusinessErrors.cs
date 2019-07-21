using Zapdate.Server.Core.Dto;

namespace Zapdate.Server.Core.Interfaces
{
    public interface IBusinessErrors
    {
        /// <summary>
        ///     The errors that occurred when executing the use case. If empty, the use case succeeded
        /// </summary>
        Error? Error { get; }

        /// <summary>
        ///     True if error occurred on executing this use case
        /// </summary>
        bool HasError { get; }
    }
}
