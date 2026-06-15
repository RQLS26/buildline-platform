namespace Buildline.Platform.Requisition.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by the create-material endpoint.
/// </summary>
/// <param name="Sku">Business SKU assigned to the material.</param>
/// <param name="Name">Material name displayed in reference views.</param>
/// <param name="Category">Category name used by filters and reports.</param>
/// <param name="Unit">Measurement unit used for requisition quantities.</param>
/// <param name="Project">Project associated with the material stock record.</param>
/// <param name="CurrentStock">Initial available stock quantity.</param>
/// <param name="MinStock">Minimum stock threshold.</param>
/// <param name="MaxStock">Maximum stock threshold.</param>
public record CreateMaterialResource(
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock);



