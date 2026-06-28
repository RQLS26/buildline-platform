using Buildline.Platform.Analytics.Domain.Model.Aggregates;

namespace Buildline.Platform.Analytics.Application.QueryServices;

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
    ///     Lists every construction project reference.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The projects available to frontend filters and dashboard views.</returns>
    Task<IEnumerable<Project>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one construction project by identifier.
    /// </summary>
    /// <param name="projectId">The identifier of the project to look up.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The project when it exists; otherwise <c>null</c>.</returns>
    Task<Project?> FindByIdAsync(int projectId, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Project>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Project?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
