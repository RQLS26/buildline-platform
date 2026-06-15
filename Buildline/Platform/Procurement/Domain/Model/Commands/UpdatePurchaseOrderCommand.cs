namespace Buildline.Platform.Procurement.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing purchase order aggregate.
/// </summary>
/// <param name="PurchaseOrderId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="OrderId">Write value for the OrderId field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <param name="SupplierName">Write value for the SupplierName field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="TotalAmount">Write value for the TotalAmount field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdatePurchaseOrderCommand(
    int PurchaseOrderId,
    string? OrderId,
    string? Date,
    string? SupplierName,
    string? Material,
    string? Project,
    decimal? TotalAmount,
    string? Status);
