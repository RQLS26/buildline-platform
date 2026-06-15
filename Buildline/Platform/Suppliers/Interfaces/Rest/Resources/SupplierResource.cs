namespace Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by supplier directory endpoints.
/// </summary>
/// <param name="Id">Supplier identifier used by the frontend.</param>
/// <param name="Ruc">Peruvian tax identifier for supplier validation.</param>
/// <param name="CompanyName">Supplier company name.</param>
/// <param name="ContactName">Primary operational contact.</param>
/// <param name="Email">Contact email for formal communications.</param>
/// <param name="Phone">Contact phone for urgent coordination.</param>
/// <param name="Rating">Supplier rating from one to five.</param>
/// <param name="IsActive">Flag indicating whether the supplier can be used.</param>
/// <param name="Category">Material category associated with the supplier.</param>
/// <param name="DeliveryRate">Historical on-time delivery percentage.</param>
public record SupplierResource(
    int Id,
    string Ruc,
    string CompanyName,
    string ContactName,
    string Email,
    string Phone,
    int Rating,
    bool IsActive,
    string Category,
    int DeliveryRate);
