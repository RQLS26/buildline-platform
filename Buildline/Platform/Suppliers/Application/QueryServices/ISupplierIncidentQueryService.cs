using Buildline.Platform.Suppliers.Domain.Model.Aggregates;

namespace Buildline.Platform.Suppliers.Application.QueryServices;

/// <summary>
///     Application query contract for supplier incident read use cases.
/// </summary>
public interface ISupplierIncidentQueryService
{
    /// <summary>
    ///     Retrieves every supplier incident used by the incident management board.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>An incident collection, possibly empty.</returns>
    Task<IEnumerable<SupplierIncident>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single incident by persistence identity.
    /// </summary>
    /// <param name="id">Incident identifier requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The incident aggregate when found; otherwise <c>null</c>.</returns>
    Task<SupplierIncident?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<SupplierIncident>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<SupplierIncident?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
