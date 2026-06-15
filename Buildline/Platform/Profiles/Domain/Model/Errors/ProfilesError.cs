namespace Buildline.Platform.Profiles.Domain.Model.Errors;

/// <summary>
///     Domain error codes emitted by the Profiles bounded context.
/// </summary>
/// <remarks>
///     REST assemblers translate these values to localized Problem Details responses.
/// </remarks>
public enum ProfilesError
{
    /// <summary>
    ///     The requested company profile identifier does not exist.
    /// </summary>
    ProfileNotFound,

    /// <summary>
    ///     Required profile data is missing or invalid.
    /// </summary>
    InvalidProfileData,

    /// <summary>
    ///     The profile operation was cancelled before completion.
    /// </summary>
    OperationCancelled,

    /// <summary>
    ///     Persistence failed while accessing profile data.
    /// </summary>
    DatabaseError,

    /// <summary>
    ///     An unexpected failure occurred in the Profiles bounded context.
    /// </summary>
    InternalServerError
}

