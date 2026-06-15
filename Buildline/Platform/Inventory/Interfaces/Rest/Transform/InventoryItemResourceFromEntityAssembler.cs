using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;

namespace Buildline.Platform.Inventory.Interfaces.Rest.Transform;

/// <summary>Assembler that maps inventory item aggregates to REST resources.</summary>
public static class InventoryItemResourceFromEntityAssembler
{
    /// <summary>Converts an inventory item aggregate into the frontend resource contract.</summary>
    /// <param name="item">Inventory item aggregate retrieved from persistence.</param>
    /// <returns>Inventory item REST resource.</returns>
    public static InventoryItemResource ToResourceFromEntity(InventoryItem item)
    {
        return new InventoryItemResource(
            item.Id,
            item.Sku,
            item.Name,
            item.Project,
            item.Category,
            item.CurrentStock,
            item.MaxStock,
            item.MinStock,
            item.LastUpdated);
    }
}
