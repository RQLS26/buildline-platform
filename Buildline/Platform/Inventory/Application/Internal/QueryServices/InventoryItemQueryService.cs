using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Repositories;

namespace Buildline.Platform.Inventory.Application.Internal.QueryServices;

/// <summary>Application query service that coordinates inventory reads.</summary>
/// <param name="inventoryItemRepository">Repository used to retrieve inventory items.</param>
public class InventoryItemQueryService(IInventoryItemRepository inventoryItemRepository) : IInventoryItemQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<InventoryItem>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await inventoryItemRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<InventoryItem?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await inventoryItemRepository.FindByIdAsync(id, cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<InventoryItem>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await inventoryItemRepository.ListByCompanyIdAsync(companyId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<InventoryItem?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await inventoryItemRepository.FindByIdAndCompanyIdAsync(id, companyId, cancellationToken);
    }
}
