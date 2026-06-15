namespace Buildline.Platform.Suppliers.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing supplier incident aggregate.
/// </summary>
/// <param name="SupplierIncidentId">Persistence identifier of the aggregate selected by the route.</param>
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
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateSupplierIncidentCommand(
    int SupplierIncidentId,
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
