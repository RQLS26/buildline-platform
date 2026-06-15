namespace Buildline.Platform.Delivery.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a delivery aggregate from a REST write payload.
/// </summary>
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
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateDeliveryCommand(
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
