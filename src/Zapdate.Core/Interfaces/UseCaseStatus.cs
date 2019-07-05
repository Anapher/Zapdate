using Zapdate.Core.Dto;

namespace Zapdate.Core.Interfaces
{
    public abstract class UseCaseStatus<TResponse> : BusinessActionStatus where TResponse : class
    {
        /// <summary>
        ///     Returns the error: adds the error to the collection and returns default(T).
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        /// <returns>Always return default(T)</returns>
        protected TResponse? ReturnError(Error error)
        {
            SetError(error);
            return default;
        }
    }
}
