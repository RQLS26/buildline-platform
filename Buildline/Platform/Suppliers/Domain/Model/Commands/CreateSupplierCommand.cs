namespace Buildline.Platform.Suppliers.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a supplier aggregate from a REST write payload.
/// </summary>
/// <param name="Ruc">Write value for the Ruc field in the frontend-compatible contract.</param>
/// <param name="CompanyName">Write value for the CompanyName field in the frontend-compatible contract.</param>
/// <param name="ContactName">Write value for the ContactName field in the frontend-compatible contract.</param>
/// <param name="Email">Write value for the Email field in the frontend-compatible contract.</param>
/// <param name="Phone">Write value for the Phone field in the frontend-compatible contract.</param>
/// <param name="Rating">Write value for the Rating field in the frontend-compatible contract.</param>
/// <param name="IsActive">Write value for the IsActive field in the frontend-compatible contract.</param>
/// <param name="Category">Write value for the Category field in the frontend-compatible contract.</param>
/// <param name="DeliveryRate">Write value for the DeliveryRate field in the frontend-compatible contract.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateSupplierCommand(
    string? Ruc,
    string? CompanyName,
    string? ContactName,
    string? Email,
    string? Phone,
    int? Rating,
    bool? IsActive,
    string? Category,
    int? DeliveryRate);
