namespace Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by supplier incident endpoints.
/// </summary>
/// <param name="Id">Incident persistence identifier.</param>
/// <param name="IncidentId">Business incident code displayed in the frontend.</param>
/// <param name="Title">Short incident title.</param>
/// <param name="Description">Detailed incident description.</param>
/// <param name="Supplier">Supplier display name associated with the incident.</param>
/// <param name="PurchaseOrder">Purchase order code associated with the incident.</param>
/// <param name="ReportedBy">Employee who reported the incident.</param>
/// <param name="Severity">Incident severity value.</param>
/// <param name="Status">Incident workflow status.</param>
/// <param name="Date">Display date.</param>
/// <param name="Time">Display time.</param>
public record IncidentResource(
    int Id,
    string IncidentId,
    string Title,
    string Description,
    string Supplier,
    string PurchaseOrder,
    string ReportedBy,
    string Severity,
    string Status,
    string Date,
    string Time);
