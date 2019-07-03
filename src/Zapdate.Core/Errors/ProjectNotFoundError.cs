using System.Collections.Generic;

namespace Zapdate.Core.Errors
{
    public class ResourceNotFoundError : DomainError
    {
        public ResourceNotFoundError(string message, ErrorCode code, IReadOnlyDictionary<string, string>? fields = null) : base(ErrorType.NotFound, message, code, fields)
        {
        }

        public static ResourceNotFoundError ProjectNotFound(int projectId) => new ResourceNotFoundError($"The project with id {projectId} was not found.", ErrorCode.ProjectNotFound);
    }
}
