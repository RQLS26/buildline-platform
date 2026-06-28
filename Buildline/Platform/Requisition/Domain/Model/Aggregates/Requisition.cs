using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Domain.Model.Events;
using Buildline.Platform.Requisition.Domain.Model.ValueObjects;
using Buildline.Platform.Shared.Domain.Model.Events;

namespace Buildline.Platform.Requisition.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a material requisition created from a construction site.
/// </summary>
public partial class Requisition : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

    /// <summary>Initializes an empty requisition instance for Entity Framework Core materialization.</summary>
    protected Requisition()
    {
        ReqId = string.Empty;
        Material = string.Empty;
        Project = string.Empty;
        Unit = string.Empty;
        Priority = string.Empty;
        Status = string.Empty;
        RequestedOn = string.Empty;
        DeliveryDate = string.Empty;
        RequestedBy = string.Empty;
    }

    /// <summary>Creates a requisition aggregate from the application command.</summary>
    /// <param name="command">Command carrying requisition values accepted by the application layer.</param>
    public Requisition(CreateRequisitionCommand command)
    {
        CompanyId = command.CompanyId;
        var materialDescriptor = MaterialDescriptor.From(command.Material, command.Unit, string.Empty);
        ReqId = string.IsNullOrWhiteSpace(command.ReqId) ? $"MR-{DateTime.UtcNow:yyyyMMddHHmmss}" : command.ReqId.Trim();
        Material = materialDescriptor.Name;
        Project = command.Project?.Trim() ?? string.Empty;
        Quantity = Math.Max(0, command.Quantity ?? 0);
        Unit = materialDescriptor.Unit;
        Priority = command.Priority?.Trim() ?? RequisitionPriority.Medium.ToString();
        Status = command.Status?.Trim() ?? RequisitionStatus.Pending.ToString();
        RequestedOn = command.RequestedOn?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        DeliveryDate = command.DeliveryDate?.Trim() ?? string.Empty;
        RequestedBy = command.RequestedBy?.Trim() ?? string.Empty;
        AddDomainEvent(new RequisitionCreatedEvent(Id, ReqId, Material, Project));
    }

    /// <summary>Gets the database-generated requisition identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

    /// <summary>Gets the business requisition code displayed in operations screens.</summary>
    public string ReqId { get; private set; }

    /// <summary>Gets the requested material name.</summary>
    public string Material { get; private set; }

    /// <summary>Gets the project requesting the material.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the requested quantity.</summary>
    public int Quantity { get; private set; }

    /// <summary>Gets the measurement unit for the requested quantity.</summary>
    public string Unit { get; private set; }

    /// <summary>Gets the operational priority assigned by the field team.</summary>
    public string Priority { get; private set; }

    /// <summary>Gets the approval workflow status.</summary>
    public string Status { get; private set; }

    /// <summary>Gets the display date when the requisition was created.</summary>
    public string RequestedOn { get; private set; }

    /// <summary>Gets the requested delivery date.</summary>
    public string DeliveryDate { get; private set; }

    /// <summary>Gets the employee who created the requisition.</summary>
    public string RequestedBy { get; private set; }

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial requisition update.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateRequisitionCommand command)
    {
        var previousStatus = Status;
        ReqId = command.ReqId is null ? ReqId : command.ReqId.Trim();
        Material = command.Material is null ? Material : command.Material.Trim();
        Project = command.Project is null ? Project : command.Project.Trim();
        Quantity = command.Quantity ?? Quantity;
        Unit = command.Unit is null ? Unit : command.Unit.Trim();
        Priority = command.Priority is null ? Priority : command.Priority.Trim();
        Status = command.Status is null ? Status : command.Status.Trim();
        RequestedOn = command.RequestedOn is null ? RequestedOn : command.RequestedOn.Trim();
        DeliveryDate = command.DeliveryDate is null ? DeliveryDate : command.DeliveryDate.Trim();
        RequestedBy = command.RequestedBy is null ? RequestedBy : command.RequestedBy.Trim();

        if (!string.Equals(previousStatus, Status, StringComparison.OrdinalIgnoreCase))
            AddDomainEvent(new RequisitionStatusChangedEvent(Id, previousStatus, Status));
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
