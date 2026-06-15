using Buildline.Platform.Inventory.Domain.Model.Aggregates;

namespace Buildline.Platform.Inventory.Application.QueryServices;

/// <summary>Application query contract for inventory read use cases.</summary>
public interface IInventoryItemQueryService
{
    /// <summary>Retrieves every inventory item.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>An inventory collection, possibly empty.</returns>
    Task<IEnumerable<InventoryItem>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one inventory item by identifier.</summary>
    /// <param name="id">Inventory item persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The inventory item when found; otherwise <c>null</c>.</returns>
    Task<InventoryItem?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}
