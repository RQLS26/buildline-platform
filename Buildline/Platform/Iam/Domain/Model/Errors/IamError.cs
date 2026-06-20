namespace Buildline.Platform.Iam.Domain.Model.Errors;

/// <summary>
///     Domain error codes emitted by the IAM bounded context.
/// </summary>
/// <remarks>
///     Errors are translated to localized Problem Details responses by IAM REST assemblers.
/// </remarks>
public enum IamError
{
    /// <summary>
    ///     The requested user identifier does not exist.
    /// </summary>
    UserNotFound,

    /// <summary>
    ///     The requested email is already assigned to another user.
    /// </summary>
    EmailAlreadyTaken,

    /// <summary>
    ///     Submitted sign-in credentials are invalid or the account is inactive.
    /// </summary>
    InvalidCredentials,

    /// <summary>
    ///     The submitted current password does not match the stored credential.
    /// </summary>
    InvalidCurrentPassword,

    /// <summary>
    ///     The submitted password does not meet minimum security requirements.
    /// </summary>
    WeakPassword,

    /// <summary>
    ///     The requested role transition is not allowed by the IAM role catalog.
    /// </summary>
    InvalidRole,

    /// <summary>
    ///     The authenticated user is not allowed to perform the requested IAM operation.
    /// </summary>
    ForbiddenOperation,

    /// <summary>
    ///     The IAM operation was cancelled before completion.
    /// </summary>
    OperationCancelled,

    /// <summary>
    ///     Persistence failed while accessing IAM data.
    /// </summary>
    DatabaseError,

    /// <summary>
    ///     An unexpected failure occurred in the IAM bounded context.
    /// </summary>
    InternalServerError
}

