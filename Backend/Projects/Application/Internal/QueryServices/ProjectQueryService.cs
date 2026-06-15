using Buildline.Platform.Projects.Application.QueryServices;
using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Buildline.Platform.Projects.Domain.Model.Queries;
using Buildline.Platform.Projects.Domain.Repositories;

namespace Buildline.Platform.Projects.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates read access to construction project references.
/// </summary>
/// <param name="projectRepository">Repository used to retrieve persisted project aggregates.</param>
/// <remarks>
///     No business mutation occurs here. The service exists to keep controllers aligned with CQRS and
///     to provide a stable extension point for future filtering or ACL-based project visibility.
/// </remarks>
public class ProjectQueryService(IProjectRepository projectRepository) : IProjectQueryService
{
    /// <summary>
    ///     Retrieves every project available to the current backend version.
    /// </summary>
    /// <param name="query">Project listing query.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A collection of project aggregates, possibly empty.</returns>
    public async Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken = default)
    {
        return await projectRepository.ListAsync(cancellationToken);
    }

    /// <summary>
    ///     Retrieves a single project by its identifier.
    /// </summary>
    /// <param name="query">Project lookup query containing the requested id.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The project aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<Project?> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await projectRepository.FindByIdAsync(query.ProjectId, cancellationToken);
    }
}
