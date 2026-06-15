using Buildline.Platform.Delivery.Domain.Model.Commands;
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
    /// <param name="command">Delivery payload submitted by the tracking workflow.</param>
    public Delivery(CreateDeliveryCommand command)
    {
        TrackingId = string.IsNullOrWhiteSpace(command.TrackingId) ? $"TRK-{DateTime.UtcNow:HHmmss}" : command.TrackingId.Trim();
        PurchaseOrder = command.PurchaseOrder?.Trim() ?? string.Empty;
        Supplier = command.Supplier?.Trim() ?? string.Empty;
        Origin = command.Origin?.Trim() ?? string.Empty;
        Destination = command.Destination?.Trim() ?? string.Empty;
        Status = command.Status?.Trim() ?? "Shipped";
        Eta = command.Eta?.Trim() ?? string.Empty;
        DispatchDate = command.DispatchDate?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        Items = command.Items?.Trim() ?? command.Material?.Trim() ?? string.Empty;
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
    /// <param name="command">Delivery fields to replace.</param>
    public void Apply(UpdateDeliveryCommand command)
    {
        TrackingId = command.TrackingId is null ? TrackingId : command.TrackingId.Trim();
        PurchaseOrder = command.PurchaseOrder is null ? PurchaseOrder : command.PurchaseOrder.Trim();
        Supplier = command.Supplier is null ? Supplier : command.Supplier.Trim();
        Origin = command.Origin is null ? Origin : command.Origin.Trim();
        Destination = command.Destination is null ? Destination : command.Destination.Trim();
        Status = command.Status is null ? Status : command.Status.Trim();
        Eta = command.Eta is null ? Eta : command.Eta.Trim();
        DispatchDate = command.DispatchDate is null ? DispatchDate : command.DispatchDate.Trim();
        Items = command.Items is null ? Items : command.Items.Trim();
    }
}



