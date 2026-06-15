namespace Buildline.Platform.Delivery.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing delivery aggregate.
/// </summary>
/// <param name="DeliveryId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="TrackingId">Write value for the TrackingId field in the frontend-compatible contract.</param>
/// <param name="PurchaseOrder">Write value for the PurchaseOrder field in the frontend-compatible contract.</param>
/// <param name="Supplier">Write value for the Supplier field in the frontend-compatible contract.</param>
/// <param name="Origin">Write value for the Origin field in the frontend-compatible contract.</param>
/// <param name="Destination">Write value for the Destination field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="Eta">Write value for the Eta field in the frontend-compatible contract.</param>
/// <param name="DispatchDate">Write value for the DispatchDate field in the frontend-compatible contract.</param>
/// <param name="Items">Write value for the Items field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateDeliveryCommand(
    int DeliveryId,
    string? TrackingId,
    string? PurchaseOrder,
    string? Supplier,
    string? Origin,
    string? Destination,
    string? Status,
    string? Eta,
    string? DispatchDate,
    string? Items,
    string? Material);
