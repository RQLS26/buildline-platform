namespace Buildline.Platform.Requisition.Domain.Model.Commands;

/// <summary>
///     Command that requests replacement of reference and stock fields for an existing material.
/// </summary>
/// <param name="MaterialId">Identifier of the material aggregate to update.</param>
/// <param name="Sku">Replacement SKU value.</param>
/// <param name="Name">Replacement material name.</param>
/// <param name="Category">Replacement category name.</param>
/// <param name="Unit">Replacement measurement unit.</param>
/// <param name="Project">Replacement project association.</param>
/// <param name="CurrentStock">Replacement current stock quantity.</param>
/// <param name="MinStock">Replacement minimum stock threshold.</param>
/// <param name="MaxStock">Replacement maximum stock threshold.</param>
/// <remarks>
///     This command supports TS-07 and is also used by the PATCH-compatible endpoint for the current
///     frontend contract.
/// </remarks>
public record UpdateMaterialCommand(
    int MaterialId,
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock);


