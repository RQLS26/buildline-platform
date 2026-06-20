namespace Buildline.Platform.Procurement.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a quotation aggregate from a REST write payload.
/// </summary>
/// <param name="QuotationId">Write value for the QuotationId field in the frontend-compatible contract.</param>
/// <param name="Supplier">Write value for the Supplier field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="Amount">Write value for the Amount field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateQuotationCommand(
    string? QuotationId,
    string? Supplier,
    string? Material,
    string? Project,
    decimal? Amount,
    string? Status,
    string? Date,
    int CompanyId = 1);
