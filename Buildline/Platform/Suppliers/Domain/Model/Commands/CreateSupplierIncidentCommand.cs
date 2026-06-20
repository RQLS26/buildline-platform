namespace Buildline.Platform.Suppliers.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a supplier incident aggregate from a REST write payload.
/// </summary>
/// <param name="IncidentId">Write value for the IncidentId field in the frontend-compatible contract.</param>
/// <param name="Title">Write value for the Title field in the frontend-compatible contract.</param>
/// <param name="Description">Write value for the Description field in the frontend-compatible contract.</param>
/// <param name="Supplier">Write value for the Supplier field in the frontend-compatible contract.</param>
/// <param name="PurchaseOrder">Write value for the PurchaseOrder field in the frontend-compatible contract.</param>
/// <param name="ReportedBy">Write value for the ReportedBy field in the frontend-compatible contract.</param>
/// <param name="Severity">Write value for the Severity field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <param name="Time">Write value for the Time field in the frontend-compatible contract.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateSupplierIncidentCommand(
    string? IncidentId,
    string? Title,
    string? Description,
    string? Supplier,
    string? PurchaseOrder,
    string? ReportedBy,
    string? Severity,
    string? Status,
    string? Date,
    string? Time,
    int CompanyId = 1);
