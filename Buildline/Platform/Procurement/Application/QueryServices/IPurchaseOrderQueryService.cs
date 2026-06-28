using Buildline.Platform.Procurement.Domain.Model.Aggregates;

namespace Buildline.Platform.Procurement.Application.QueryServices;

/// <summary>
///     Application query contract for purchase order read use cases.
/// </summary>
public interface IPurchaseOrderQueryService
{
    /// <summary>Retrieves every purchase order.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A purchase order collection, possibly empty.</returns>
    Task<IEnumerable<PurchaseOrder>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one purchase order by identifier.</summary>
    /// <param name="id">Purchase order persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The purchase order when found; otherwise <c>null</c>.</returns>
    Task<PurchaseOrder?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<PurchaseOrder>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<PurchaseOrder?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
