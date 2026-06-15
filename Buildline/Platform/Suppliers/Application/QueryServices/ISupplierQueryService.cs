using Buildline.Platform.Suppliers.Domain.Model.Aggregates;

namespace Buildline.Platform.Suppliers.Application.QueryServices;

/// <summary>
///     Application query contract for supplier directory read use cases.
/// </summary>
public interface ISupplierQueryService
{
    /// <summary>
    ///     Retrieves every supplier known by the company.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A supplier collection, possibly empty.</returns>
    Task<IEnumerable<Supplier>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a single supplier by persistence identity.
    /// </summary>
    /// <param name="id">Supplier identifier requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The supplier aggregate when found; otherwise <c>null</c>.</returns>
    Task<Supplier?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}
