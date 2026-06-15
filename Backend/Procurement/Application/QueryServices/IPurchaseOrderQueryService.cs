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
}
