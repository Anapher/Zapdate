using Zapdate.Core.Errors;

namespace Zapdate.Models.Errors
{
    public class UrlParameterValidationError : DomainError
    {
        public UrlParameterValidationError(string message) : base(ErrorType.ValidationError, message, (ErrorCode) 1)
        {
        }
    }
}
