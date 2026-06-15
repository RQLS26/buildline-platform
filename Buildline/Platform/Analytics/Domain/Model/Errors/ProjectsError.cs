namespace Buildline.Platform.Analytics.Domain.Model.Errors;

/// <summary>
///     Domain error codes emitted by the Analytics reference data module.
/// </summary>
/// <remarks>
///     Errors are translated to localized messages through shared resources and then to HTTP
///     Problem Details by the REST action result assembler.
/// </remarks>
public enum ProjectsError
{
    /// <summary>
    ///     The requested project identifier does not exist.
    /// </summary>
    ProjectNotFound,

    /// <summary>
    ///     The project payload or state violates context rules.
    /// </summary>
    InvalidProjectData,

    /// <summary>
    ///     The operation was cancelled before completion.
    /// </summary>
    OperationCancelled,

    /// <summary>
    ///     Persistence failed while accessing project data.
    /// </summary>
    DatabaseError,

    /// <summary>
    ///     An unexpected failure occurred in the Analytics reference data module.
    /// </summary>
    InternalServerError
}


