namespace Buildline.Platform.Delivery.Interfaces.Rest.Resources;

/// <summary>Resource accepted by delivery create and partial update endpoints.</summary>
public record DeliveryWriteResource(
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
