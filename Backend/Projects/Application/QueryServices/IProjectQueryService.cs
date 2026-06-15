using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Buildline.Platform.Projects.Domain.Model.Queries;

namespace Buildline.Platform.Projects.Application.QueryServices;

/// <summary>
///     Defines read operations exposed by the Projects application layer.
/// </summary>
/// <remarks>
///     Query services keep REST controllers independent from repository implementations and preserve
///     the CQRS split used across the course sample backend.
/// </remarks>
public interface IProjectQueryService
{
    /// <summary>
    ///     Handles the query that retrieves every construction project reference.
    /// </summary>
    /// <param name="query">Query object representing the project listing request.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The projects available to frontend filters and dashboard views.</returns>
    Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves one construction project by identifier.
    /// </summary>
    /// <param name="query">Query object containing the requested project id.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The project when it exists; otherwise <c>null</c>.</returns>
    Task<Project?> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken = default);
}
