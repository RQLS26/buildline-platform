namespace Buildline.Platform.Materials.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by material update endpoints.
/// </summary>
/// <param name="Sku">Replacement SKU value.</param>
/// <param name="Name">Replacement material name.</param>
/// <param name="Category">Replacement category name.</param>
/// <param name="Unit">Replacement measurement unit.</param>
/// <param name="Project">Replacement project association.</param>
/// <param name="CurrentStock">Replacement current stock quantity.</param>
/// <param name="MinStock">Replacement minimum stock threshold.</param>
/// <param name="MaxStock">Replacement maximum stock threshold.</param>
/// <remarks>
///     The current PATCH endpoint accepts this same resource to stay compatible with the frontend
///     material form, which sends complete material payloads.
/// </remarks>
public record UpdateMaterialResource(
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock);
