using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Domain.Model.Events;
using Buildline.Platform.Procurement.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Procurement.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a purchase order emitted by the procurement workflow.
/// </summary>
public partial class PurchaseOrder : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

    /// <summary>Initializes an empty purchase order for Entity Framework Core materialization.</summary>
    protected PurchaseOrder()
    {
        OrderId = string.Empty;
        Date = string.Empty;
        SupplierName = string.Empty;
        Material = string.Empty;
        Project = string.Empty;
        Status = string.Empty;
    }

    /// <summary>Creates a purchase order from a procurement command.</summary>
    /// <param name="command">Command carrying purchase order values accepted by the application layer.</param>
    public PurchaseOrder(CreatePurchaseOrderCommand command)
    {
        CompanyId = command.CompanyId;
        OrderId = string.IsNullOrWhiteSpace(command.OrderId) ? $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}" : command.OrderId.Trim();
        Date = command.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        SupplierName = command.SupplierName?.Trim() ?? string.Empty;
        Material = command.Material?.Trim() ?? string.Empty;
        Project = command.Project?.Trim() ?? string.Empty;
        TotalAmount = Money.From(command.TotalAmount).Amount;
        Status = command.Status?.Trim() ?? ProcurementStatus.Pending.ToString();
        AddDomainEvent(new PurchaseOrderCreatedEvent(Id, OrderId, SupplierName, TotalAmount));
    }

    /// <summary>Gets the database-generated purchase order identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

    /// <summary>Gets the business purchase order code.</summary>
    public string OrderId { get; private set; }

    /// <summary>Gets the display date used by procurement history views.</summary>
    public string Date { get; private set; }

    /// <summary>Gets the supplier display name.</summary>
    public string SupplierName { get; private set; }

    /// <summary>Gets the purchased material description.</summary>
    public string Material { get; private set; }

    /// <summary>Gets the project charged by this purchase order.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the total purchase amount.</summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>Gets the approval status used by the approval inbox.</summary>
    public string Status { get; private set; }

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial purchase order update.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdatePurchaseOrderCommand command)
    {
        var previousStatus = Status;
        OrderId = command.OrderId is null ? OrderId : command.OrderId.Trim();
        Date = command.Date is null ? Date : command.Date.Trim();
        SupplierName = command.SupplierName is null ? SupplierName : command.SupplierName.Trim();
        Material = command.Material is null ? Material : command.Material.Trim();
        Project = command.Project is null ? Project : command.Project.Trim();
        TotalAmount = command.TotalAmount is null ? TotalAmount : Money.From(command.TotalAmount).Amount;
        Status = command.Status is null ? Status : command.Status.Trim();

        if (!string.Equals(previousStatus, Status, StringComparison.OrdinalIgnoreCase))
            AddDomainEvent(new PurchaseOrderStatusChangedEvent(Id, previousStatus, Status));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
