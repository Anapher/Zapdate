namespace Zapdate.Core.Errors
{
    public class UpdatePackageAlreadyExistsError : DomainError
    {
        public UpdatePackageAlreadyExistsError() : base(ErrorType.InvalidOperation, "An update package with the same version already exists.", ErrorCode.UpdatePackageWithVersionAlreadyExists)
        {
        }
    }
}
