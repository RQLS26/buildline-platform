using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Inventory.Domain.Model.Events;

/// <summary>
///     Domain event raised when InventoryStockChangedEvent occurs inside the aggregate boundary.
/// </summary>
public sealed record InventoryStockChangedEvent(int InventoryItemId, string Sku, int PreviousStock, int CurrentStock, bool IsBelowMinimum) : IEvent;
