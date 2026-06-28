using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Delivery.Domain.Model.Events;
using Buildline.Platform.Delivery.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Delivery.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a tracked delivery linked to a purchase order.
/// </summary>
public partial class Delivery : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

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

    /// <summary>Creates a delivery aggregate from a tracking command.</summary>
    /// <param name="command">Command carrying delivery values accepted by the application layer.</param>
    public Delivery(CreateDeliveryCommand command)
    {
        CompanyId = command.CompanyId;
        TrackingId = TrackingCode.From(command.TrackingId).Value;
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

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

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

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial delivery update, especially status changes.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateDeliveryCommand command)
    {
        var previousStatus = Status;
        TrackingId = command.TrackingId is null ? TrackingId : command.TrackingId.Trim();
        PurchaseOrder = command.PurchaseOrder is null ? PurchaseOrder : command.PurchaseOrder.Trim();
        Supplier = command.Supplier is null ? Supplier : command.Supplier.Trim();
        Origin = command.Origin is null ? Origin : command.Origin.Trim();
        Destination = command.Destination is null ? Destination : command.Destination.Trim();
        Status = command.Status is null ? Status : command.Status.Trim();
        Eta = command.Eta is null ? Eta : command.Eta.Trim();
        DispatchDate = command.DispatchDate is null ? DispatchDate : command.DispatchDate.Trim();
        Items = command.Items is null ? Items : command.Items.Trim();

        if (!string.Equals(previousStatus, Status, StringComparison.OrdinalIgnoreCase))
            AddDomainEvent(new DeliveryStatusChangedEvent(Id, TrackingId, previousStatus, Status));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
