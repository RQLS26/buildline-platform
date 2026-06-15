namespace Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by supplier create and partial update endpoints.
/// </summary>
/// <param name="Ruc">Supplier RUC. Required on creation and optional on PATCH.</param>
/// <param name="CompanyName">Supplier company name. Required on creation and optional on PATCH.</param>
/// <param name="ContactName">Primary contact name.</param>
/// <param name="Email">Supplier contact email.</param>
/// <param name="Phone">Supplier contact phone.</param>
/// <param name="Rating">Supplier rating from one to five.</param>
/// <param name="IsActive">Supplier activation flag.</param>
/// <param name="Category">Supplier category used by filters.</param>
/// <param name="DeliveryRate">Historical delivery performance percentage.</param>
public record SupplierWriteResource(
    string? Ruc,
    string? CompanyName,
    string? ContactName,
    string? Email,
    string? Phone,
    int? Rating,
    bool? IsActive,
    string? Category,
    int? DeliveryRate);
