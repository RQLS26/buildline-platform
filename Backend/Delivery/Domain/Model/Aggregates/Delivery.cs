using Buildline.Platform.Delivery.Interfaces.Rest.Resources;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Delivery.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a tracked delivery linked to a purchase order.
/// </summary>
/// <remarks>
///     Delivery and Tracking owns dispatch state, origin, destination and ETA data. It deliberately
///     references purchase orders by business code to preserve the frontend tracking contract while
///     avoiding direct coupling to Procurement aggregate internals.
/// </remarks>
public partial class Delivery : IAuditableEntity
{
    /// <summary>Initializes an empty delivery for Entity Framework Core materialization.</summary>
    protected Delivery()
    {
        TrackingId = string.Empty;
        PurchaseOrder = string.Empty;
        Supplier = string.Empty;
        Origin = string.Empty;
        Destination = string.Empty;
        Status = string.Empty;
        Eta = string.Empty;
        DispatchDate = string.Empty;
        Items = string.Empty;
    }

    /// <summary>Creates a delivery aggregate from the frontend tracking contract.</summary>
    /// <param name="resource">Delivery payload submitted by the tracking workflow.</param>
    public Delivery(DeliveryWriteResource resource)
    {
        TrackingId = string.IsNullOrWhiteSpace(resource.TrackingId) ? $"TRK-{DateTime.UtcNow:HHmmss}" : resource.TrackingId.Trim();
        PurchaseOrder = resource.PurchaseOrder?.Trim() ?? string.Empty;
        Supplier = resource.Supplier?.Trim() ?? string.Empty;
        Origin = resource.Origin?.Trim() ?? string.Empty;
        Destination = resource.Destination?.Trim() ?? string.Empty;
        Status = resource.Status?.Trim() ?? "Shipped";
        Eta = resource.Eta?.Trim() ?? string.Empty;
        DispatchDate = resource.DispatchDate?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        Items = resource.Items?.Trim() ?? resource.Material?.Trim() ?? string.Empty;
    }

    /// <summary>Gets the database-generated delivery identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the business tracking code.</summary>
    public string TrackingId { get; private set; }

    /// <summary>Gets the related purchase order code.</summary>
    public string PurchaseOrder { get; private set; }

    /// <summary>Gets the supplier dispatching the delivery.</summary>
    public string Supplier { get; private set; }

    /// <summary>Gets the delivery origin.</summary>
    public string Origin { get; private set; }

    /// <summary>Gets the delivery destination.</summary>
    public string Destination { get; private set; }

    /// <summary>Gets the current delivery status.</summary>
    public string Status { get; private set; }

    /// <summary>Gets the estimated arrival display value.</summary>
    public string Eta { get; private set; }

    /// <summary>Gets the dispatch date display value.</summary>
    public string DispatchDate { get; private set; }

    /// <summary>Gets the item summary shown in tracking lists.</summary>
    public string Items { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the delivery is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the delivery is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>Applies a partial delivery update, especially status changes.</summary>
    /// <param name="resource">Delivery fields to replace.</param>
    public void Apply(DeliveryWriteResource resource)
    {
        TrackingId = resource.TrackingId is null ? TrackingId : resource.TrackingId.Trim();
        PurchaseOrder = resource.PurchaseOrder is null ? PurchaseOrder : resource.PurchaseOrder.Trim();
        Supplier = resource.Supplier is null ? Supplier : resource.Supplier.Trim();
        Origin = resource.Origin is null ? Origin : resource.Origin.Trim();
        Destination = resource.Destination is null ? Destination : resource.Destination.Trim();
        Status = resource.Status is null ? Status : resource.Status.Trim();
        Eta = resource.Eta is null ? Eta : resource.Eta.Trim();
        DispatchDate = resource.DispatchDate is null ? DispatchDate : resource.DispatchDate.Trim();
        Items = resource.Items is null ? Items : resource.Items.Trim();
    }
}
