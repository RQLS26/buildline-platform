namespace Buildline.Platform.Inventory.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by inventory endpoints.
/// </summary>
public record InventoryItemResource(
    int Id,
    string Sku,
    string Name,
    string Project,
    string Category,
    int CurrentStock,
    int MaxStock,
    int MinStock,
    string LastUpdated);
