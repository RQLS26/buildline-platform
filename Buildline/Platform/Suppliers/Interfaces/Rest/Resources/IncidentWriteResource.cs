namespace Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by supplier incident create and partial update endpoints.
/// </summary>
/// <param name="IncidentId">Business incident code.</param>
/// <param name="Title">Short incident title.</param>
/// <param name="Description">Detailed incident description.</param>
/// <param name="Supplier">Supplier display name.</param>
/// <param name="PurchaseOrder">Related purchase order code.</param>
/// <param name="ReportedBy">Employee who reported the incident.</param>
/// <param name="Severity">Operational severity value.</param>
/// <param name="Status">Workflow status value.</param>
/// <param name="Date">Display date.</param>
/// <param name="Time">Display time.</param>
public record IncidentWriteResource(
    string? IncidentId,
    string? Title,
    string? Description,
    string? Supplier,
    string? PurchaseOrder,
    string? ReportedBy,
    string? Severity,
    string? Status,
    string? Date,
    string? Time);
