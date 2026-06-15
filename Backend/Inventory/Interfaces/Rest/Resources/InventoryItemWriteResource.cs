namespace Buildline.Platform.Inventory.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by inventory create and partial update endpoints.
/// </summary>
public record InventoryItemWriteResource(
    string? Sku,
    string? Name,
    string? Project,
    string? Category,
    int? CurrentStock,
    int? MaxStock,
    int? MinStock,
    string? LastUpdated);
