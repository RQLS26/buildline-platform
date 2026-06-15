using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

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
    ///     Creates an incident aggregate from the supplier incident frontend contract.
    /// </summary>
    /// <param name="resource">Incident payload submitted by the supplier incidents screen.</param>
    public SupplierIncident(IncidentWriteResource resource)
    {
        IncidentId = string.IsNullOrWhiteSpace(resource.IncidentId) ? "INC-DRAFT" : resource.IncidentId.Trim();
        Title = resource.Title?.Trim() ?? string.Empty;
        Description = resource.Description?.Trim() ?? string.Empty;
        Supplier = resource.Supplier?.Trim() ?? string.Empty;
        PurchaseOrder = resource.PurchaseOrder?.Trim() ?? string.Empty;
        ReportedBy = resource.ReportedBy?.Trim() ?? string.Empty;
        Severity = resource.Severity?.Trim() ?? "Medium";
        Status = resource.Status?.Trim() ?? "Open";
        Date = resource.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        Time = resource.Time?.Trim() ?? DateTime.UtcNow.ToString("HH:mm");
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
    /// <param name="resource">Resource containing the incident fields to change.</param>
    public void Apply(IncidentWriteResource resource)
    {
        IncidentId = resource.IncidentId is null ? IncidentId : resource.IncidentId.Trim();
        Title = resource.Title is null ? Title : resource.Title.Trim();
        Description = resource.Description is null ? Description : resource.Description.Trim();
        Supplier = resource.Supplier is null ? Supplier : resource.Supplier.Trim();
        PurchaseOrder = resource.PurchaseOrder is null ? PurchaseOrder : resource.PurchaseOrder.Trim();
        ReportedBy = resource.ReportedBy is null ? ReportedBy : resource.ReportedBy.Trim();
        Severity = resource.Severity is null ? Severity : resource.Severity.Trim();
        Status = resource.Status is null ? Status : resource.Status.Trim();
        Date = resource.Date is null ? Date : resource.Date.Trim();
        Time = resource.Time is null ? Time : resource.Time.Trim();
    }
}
