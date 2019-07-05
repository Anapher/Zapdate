using Zapdate.Core.Dto;

namespace Zapdate.Core.Interfaces
{
    public class BusinessActionStatus : IBusinessErrors
    {
        /// <summary>
        ///     The errors that occurred when executing this UseCase. If empty, the UseCase succeeded
        /// </summary>
        public Error? Error { get; set; }

        /// <summary>
        ///     Returns true if <see cref="Error"/> is not null
        /// </summary>
        public bool HasError => Error != null;

        /// <summary>
        ///     This adds one error to the Errors collection
        /// </summary>
        /// <param name="error">The error that should be added</param>
        protected void SetError(Error error)
        {
            Error = error;
        }

        /// <summary>
        ///     Inherit errors from another business action status and return true if that action failed
        /// </summary>
        /// <param name="status">The status that should be inherited</param>
        /// <returns>Return true if the other status failed</returns>
        protected bool InheritError(IBusinessErrors status)
        {
            if (status.HasError)
            {
                SetError(status.Error!);
                return true;
            }

            return false;
        }
    }
}
