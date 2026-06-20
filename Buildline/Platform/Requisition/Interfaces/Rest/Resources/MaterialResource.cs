namespace Buildline.Platform.Requisition.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by material reference endpoints.
/// </summary>
/// <param name="CompanyId">Company profile identifier that owns the resource.</param>
/// <param name="Id">Material identifier used by API clients.</param>
/// <param name="Sku">Business SKU displayed in material tables.</param>
/// <param name="Name">Material display name.</param>
/// <param name="Category">Category name used by filters.</param>
/// <param name="Unit">Measurement unit used for stock quantities.</param>
/// <param name="Project">Project associated with the material stock record.</param>
/// <param name="CurrentStock">Current available stock quantity.</param>
/// <param name="MinStock">Minimum stock threshold.</param>
/// <param name="MaxStock">Maximum stock threshold.</param>
public record MaterialResource(
    int Id,
    int CompanyId,
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock);


