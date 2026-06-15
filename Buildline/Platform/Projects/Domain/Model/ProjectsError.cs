namespace Buildline.Platform.Projects.Domain.Model;

/// <summary>
///     Domain error codes emitted by the Projects bounded context.
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
    ///     An unexpected failure occurred in the Projects bounded context.
    /// </summary>
    InternalServerError
}
