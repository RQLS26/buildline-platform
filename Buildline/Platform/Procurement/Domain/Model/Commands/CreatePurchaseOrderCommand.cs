namespace Buildline.Platform.Procurement.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a purchase order aggregate from a REST write payload.
/// </summary>
/// <param name="OrderId">Write value for the OrderId field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <param name="SupplierName">Write value for the SupplierName field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="TotalAmount">Write value for the TotalAmount field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreatePurchaseOrderCommand(
    string? OrderId,
    string? Date,
    string? SupplierName,
    string? Material,
    string? Project,
    decimal? TotalAmount,
    string? Status,
    int CompanyId = 1);
