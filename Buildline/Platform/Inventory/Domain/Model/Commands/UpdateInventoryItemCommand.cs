namespace Buildline.Platform.Inventory.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing inventory item aggregate.
/// </summary>
/// <param name="InventoryItemId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="Sku">Write value for the Sku field in the frontend-compatible contract.</param>
/// <param name="Name">Write value for the Name field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="Category">Write value for the Category field in the frontend-compatible contract.</param>
/// <param name="CurrentStock">Write value for the CurrentStock field in the frontend-compatible contract.</param>
/// <param name="MaxStock">Write value for the MaxStock field in the frontend-compatible contract.</param>
/// <param name="MinStock">Write value for the MinStock field in the frontend-compatible contract.</param>
/// <param name="LastUpdated">Write value for the LastUpdated field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateInventoryItemCommand(
    int InventoryItemId,
    string? Sku,
    string? Name,
    string? Project,
    string? Category,
    int? CurrentStock,
    int? MaxStock,
    int? MinStock,
    string? LastUpdated);
