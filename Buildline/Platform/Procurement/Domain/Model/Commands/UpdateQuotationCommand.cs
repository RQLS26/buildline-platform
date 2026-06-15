namespace Buildline.Platform.Procurement.Domain.Model.Commands;

/// <summary>
///     Command that requests a partial update for an existing quotation aggregate.
/// </summary>
/// <param name="QuotationPersistenceId">Persistence identifier of the aggregate selected by the route.</param>
/// <param name="QuotationId">Write value for the QuotationId field in the frontend-compatible contract.</param>
/// <param name="Supplier">Write value for the Supplier field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="Amount">Write value for the Amount field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="Date">Write value for the Date field in the frontend-compatible contract.</param>
/// <remarks>
///     Nullable fields preserve PATCH semantics: omitted properties keep the current aggregate value.
/// </remarks>
public record UpdateQuotationCommand(
    int QuotationPersistenceId,
    string? QuotationId,
    string? Supplier,
    string? Material,
    string? Project,
    decimal? Amount,
    string? Status,
    string? Date);
