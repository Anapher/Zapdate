using System.Collections.Generic;

namespace Zapdate.Server.Core.Errors
{
    public class ResourceNotFoundError : DomainError
    {
        public ResourceNotFoundError(string message, ErrorCode code, IReadOnlyDictionary<string, string>? fields = null) : base(ErrorType.NotFound, message, code, fields)
        {
        }

        public static ResourceNotFoundError ProjectNotFound(int projectId) =>
            new ResourceNotFoundError($"The project with id {projectId} was not found.", ErrorCode.ProjectNotFound);

        public static ResourceNotFoundError UpdatePackageNotFound(int projectId, string version) =>
            new ResourceNotFoundError($"The update package {version} of project {projectId} was not found.", ErrorCode.UpdatePackageNotFound);
    }
}
