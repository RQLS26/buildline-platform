namespace Buildline.Platform.Inventory.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a inventory item aggregate from a REST write payload.
/// </summary>
/// <param name="Sku">Write value for the Sku field in the frontend-compatible contract.</param>
/// <param name="Name">Write value for the Name field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="Category">Write value for the Category field in the frontend-compatible contract.</param>
/// <param name="CurrentStock">Write value for the CurrentStock field in the frontend-compatible contract.</param>
/// <param name="MaxStock">Write value for the MaxStock field in the frontend-compatible contract.</param>
/// <param name="MinStock">Write value for the MinStock field in the frontend-compatible contract.</param>
/// <param name="LastUpdated">Write value for the LastUpdated field in the frontend-compatible contract.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateInventoryItemCommand(
    string? Sku,
    string? Name,
    string? Project,
    string? Category,
    int? CurrentStock,
    int? MaxStock,
    int? MinStock,
    string? LastUpdated,
    int CompanyId = 1);
