using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Inventory.Domain.Repositories;

/// <summary>Repository contract for inventory item aggregate persistence.</summary>
public interface IInventoryItemRepository : IBaseRepository<InventoryItem>;
