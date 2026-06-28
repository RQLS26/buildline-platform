using Buildline.Platform.Analytics.Domain.Model.Aggregates;

namespace Buildline.Platform.Analytics.Application.QueryServices;

/// <summary>Application query contract for budgeting read use cases.</summary>
public interface IBudgetQueryService
{
    /// <summary>Retrieves every project budget row.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A budget collection, possibly empty.</returns>
    Task<IEnumerable<Budget>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one budget row by identifier.</summary>
    /// <param name="id">Budget persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The budget when found; otherwise <c>null</c>.</returns>
    Task<Budget?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Budget>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Budget?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
