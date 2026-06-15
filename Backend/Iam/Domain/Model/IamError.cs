namespace Buildline.Platform.Iam.Domain.Model;

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
