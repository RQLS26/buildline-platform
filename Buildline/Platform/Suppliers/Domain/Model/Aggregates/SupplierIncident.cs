using Buildline.Platform.Shared.Domain.Model.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Buildline.Platform.Shared.Domain.Model.Events;
using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Domain.Model.Events;

namespace Buildline.Platform.Suppliers.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that captures an operational incident reported against a supplier.
/// </summary>
public partial class SupplierIncident : IHasDomainEvents
{
    private readonly List<IEvent> _domainEvents = [];

    /// <summary>Initializes an empty incident instance for Entity Framework Core materialization.</summary>
    protected SupplierIncident()
    {
        IncidentId = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Supplier = string.Empty;
        PurchaseOrder = string.Empty;
        ReportedBy = string.Empty;
        Severity = string.Empty;
        Status = string.Empty;
        Date = string.Empty;
        Time = string.Empty;
    }

    /// <summary>Creates an incident aggregate from a supplier incident command.</summary>
    /// <param name="command">Command carrying incident values accepted by the application layer.</param>
    public SupplierIncident(CreateSupplierIncidentCommand command)
    {
        CompanyId = command.CompanyId;
        IncidentId = string.IsNullOrWhiteSpace(command.IncidentId) ? "INC-DRAFT" : command.IncidentId.Trim();
        Title = command.Title?.Trim() ?? string.Empty;
        Description = command.Description?.Trim() ?? string.Empty;
        Supplier = command.Supplier?.Trim() ?? string.Empty;
        PurchaseOrder = command.PurchaseOrder?.Trim() ?? string.Empty;
        ReportedBy = command.ReportedBy?.Trim() ?? string.Empty;
        Severity = command.Severity?.Trim() ?? "Medium";
        Status = command.Status?.Trim() ?? "Open";
        Date = command.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        Time = command.Time?.Trim() ?? DateTime.UtcNow.ToString("HH:mm");
        AddDomainEvent(new SupplierIncidentReportedEvent(Id, IncidentId, Supplier, Severity));
    }

    /// <summary>Gets the database-generated incident identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

    /// <summary>Gets the business incident code shown in operational lists.</summary>
    public string IncidentId { get; private set; }

    /// <summary>Gets the short incident title.</summary>
    public string Title { get; private set; }

    /// <summary>Gets the detailed incident description.</summary>
    public string Description { get; private set; }

    /// <summary>Gets the supplier display name associated with the incident.</summary>
    public string Supplier { get; private set; }

    /// <summary>Gets the purchase order code related to the incident.</summary>
    public string PurchaseOrder { get; private set; }

    /// <summary>Gets the employee who reported the incident.</summary>
    public string ReportedBy { get; private set; }

    /// <summary>Gets the operational severity value.</summary>
    public string Severity { get; private set; }

    /// <summary>Gets the workflow status used by the incident board.</summary>
    public string Status { get; private set; }

    /// <summary>Gets the frontend display date.</summary>
    public string Date { get; private set; }

    /// <summary>Gets the frontend display time.</summary>
    public string Time { get; private set; }

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>Applies a partial incident update.</summary>
    /// <param name="command">Command containing replacement values.</param>
    public void Apply(UpdateSupplierIncidentCommand command)
    {
        IncidentId = command.IncidentId is null ? IncidentId : command.IncidentId.Trim();
        Title = command.Title is null ? Title : command.Title.Trim();
        Description = command.Description is null ? Description : command.Description.Trim();
        Supplier = command.Supplier is null ? Supplier : command.Supplier.Trim();
        PurchaseOrder = command.PurchaseOrder is null ? PurchaseOrder : command.PurchaseOrder.Trim();
        ReportedBy = command.ReportedBy is null ? ReportedBy : command.ReportedBy.Trim();
        Severity = command.Severity is null ? Severity : command.Severity.Trim();
        Status = command.Status is null ? Status : command.Status.Trim();
        Date = command.Date is null ? Date : command.Date.Trim();
        Time = command.Time is null ? Time : command.Time.Trim();
    }

    /// <summary>Records a domain event raised by this aggregate.</summary>
    /// <param name="domainEvent">Event that describes a completed domain change.</param>
    private void AddDomainEvent(IEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
