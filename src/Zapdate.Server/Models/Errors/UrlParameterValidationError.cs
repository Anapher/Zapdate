using Zapdate.Server.Core.Errors;

namespace Zapdate.Server.Models.Errors
{
    public class UrlParameterValidationError : DomainError
    {
        public UrlParameterValidationError(string message) : base(ErrorType.ValidationError, message, (ErrorCode) 1)
        {
        }
    }
}
