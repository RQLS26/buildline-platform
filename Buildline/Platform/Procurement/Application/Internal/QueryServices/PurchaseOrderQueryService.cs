using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Repositories;

namespace Buildline.Platform.Procurement.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates purchase order reads.
/// </summary>
/// <param name="purchaseOrderRepository">Repository used to retrieve purchase orders.</param>
public class PurchaseOrderQueryService(IPurchaseOrderRepository purchaseOrderRepository) : IPurchaseOrderQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<PurchaseOrder>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await purchaseOrderRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PurchaseOrder?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await purchaseOrderRepository.FindByIdAsync(id, cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<PurchaseOrder>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await purchaseOrderRepository.ListByCompanyIdAsync(companyId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PurchaseOrder?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await purchaseOrderRepository.FindByIdAndCompanyIdAsync(id, companyId, cancellationToken);
    }
}
