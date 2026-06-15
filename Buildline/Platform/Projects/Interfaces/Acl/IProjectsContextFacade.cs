namespace Buildline.Platform.Projects.Interfaces.Acl;

/// <summary>
///     Anti-corruption facade exposed by the Projects bounded context.
/// </summary>
/// <remarks>
///     Requisitions, Materials, Procurement and Analytics can use this facade to resolve project
///     reference data without depending on the Projects aggregate or EF Core model.
/// </remarks>
public interface IProjectsContextFacade
{
    /// <summary>
    ///     Fetches a project identifier by project name.
    /// </summary>
    /// <param name="projectName">Project name used as the lookup value.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The project id when the name exists; otherwise <c>0</c>.</returns>
    Task<int> FetchProjectIdByName(string projectName, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a project name by identifier.
    /// </summary>
    /// <param name="projectId">Identifier of the project requested by another bounded context.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The project name when the project exists; otherwise an empty string.</returns>
    Task<string> FetchProjectNameById(int projectId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a project status by identifier.
    /// </summary>
    /// <param name="projectId">Identifier of the project requested by another bounded context.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The project status when the project exists; otherwise an empty string.</returns>
    Task<string> FetchProjectStatusById(int projectId, CancellationToken cancellationToken = default);
}
