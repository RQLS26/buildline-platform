namespace Buildline.Platform.Delivery.Interfaces.Rest.Resources;

/// <summary>REST resource returned by delivery endpoints.</summary>
public record DeliveryResource(
    int Id,
    int CompanyId,
    string TrackingId,
    string PurchaseOrder,
    string Supplier,
    string Origin,
    string Destination,
    string Status,
    string Eta,
    string DispatchDate,
    string Items);
