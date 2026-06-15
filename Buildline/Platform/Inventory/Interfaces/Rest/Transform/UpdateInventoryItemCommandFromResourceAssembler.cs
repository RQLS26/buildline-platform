using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;

namespace Buildline.Platform.Inventory.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateInventoryItemCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="inventoryItemId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateInventoryItemCommand ToCommandFromResource(int inventoryItemId, InventoryItemWriteResource resource)
    {
        return new UpdateInventoryItemCommand(
            inventoryItemId,
            resource.Sku,
            resource.Name,
            resource.Project,
            resource.Category,
            resource.CurrentStock,
            resource.MaxStock,
            resource.MinStock,
            resource.LastUpdated);
    }
}
