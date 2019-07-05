namespace Zapdate.Core.Errors
{
    public enum ErrorCode
    {
        // code 0 to 1000 are reserved for infrastructure

        FieldValidation = 1000,
        UserNotFound,
        InvalidPassword,
        InvalidToken,
        InvalidKeyPassword,
        UpdatePackageWithVersionAlreadyExists,
        ProjectNotFound,
        UpdatePackageNotFound,

        Identity_DefaultError = 1500,
        Identity_ConcurrencyFailure,
        Identity_PasswordMismatch,
        Identity_InvalidToken,
        Identity_LoginAlreadyAssociated,
        Identity_InvalidUserName,
        Identity_InvalidEmail,
        Identity_DuplicateUserName,
        Identity_DuplicateEmail,
        Identity_InvalidRoleName,
        Identity_DuplicateRoleName,
        Identity_UserAlreadyHasPassword,
        Identity_UserLockoutNotEnabled,
        Identity_UserAlreadyInRole,
        Identity_UserNotInRole,
        Identity_PasswordTooShort,
        Identity_PasswordRequiresNonAlphanumeric,
        Identity_PasswordRequiresDigit,
        Identity_PasswordRequiresLower,
        Identity_PasswordRequiresUpper,

        FileNotFound = 2000,
    }
}
