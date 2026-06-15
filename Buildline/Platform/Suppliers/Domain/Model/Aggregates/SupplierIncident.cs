using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Suppliers.Domain.Model.Commands;

namespace Buildline.Platform.Suppliers.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that captures an operational incident reported against a supplier.
/// </summary>
/// <remarks>
///     Incidents preserve evidence for supplier evaluation and procurement risk decisions. The
///     aggregate intentionally stores the purchase-order display code used by the frontend so the
///     Sprint 2 mock contract can migrate to the API without changing the user interface.
/// </remarks>
public partial class SupplierIncident : IAuditableEntity
{
    /// <summary>
    ///     Initializes an empty incident instance for Entity Framework Core materialization.
    /// </summary>
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

    /// <summary>
    ///     Creates an incident aggregate from the supplier incident application command contract.
    /// </summary>
    /// <param name="command">Incident payload submitted by the supplier incidents screen.</param>
    public SupplierIncident(CreateSupplierIncidentCommand command)
    {
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
    }

    /// <summary>Gets the database-generated incident identifier.</summary>
    public int Id { get; private set; }

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

    /// <summary>Gets or sets the audit timestamp captured when the incident is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the incident is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Applies a partial incident update, including status transitions from the incidents board.
    /// </summary>
    /// <param name="command">Command containing the incident fields to change.</param>
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
}



