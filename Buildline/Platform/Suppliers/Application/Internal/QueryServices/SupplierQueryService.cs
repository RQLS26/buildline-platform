using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Repositories;

namespace Buildline.Platform.Suppliers.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates supplier directory reads.
/// </summary>
/// <param name="supplierRepository">Repository used to retrieve supplier aggregates.</param>
public class SupplierQueryService(ISupplierRepository supplierRepository) : ISupplierQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Supplier>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await supplierRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Supplier?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await supplierRepository.FindByIdAsync(id, cancellationToken);
    }
}
