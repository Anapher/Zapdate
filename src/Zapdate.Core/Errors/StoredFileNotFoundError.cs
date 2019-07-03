using CodeElements.Core;

namespace Zapdate.Core.Errors
{
    public class StoredFileNotFoundError : DomainError
    {
        public StoredFileNotFoundError(Hash hash) : base(ErrorType.InvalidOperation, $"The file ${hash} was not found on the server.", ErrorCode.FileNotFound)
        {
        }
    }
}
