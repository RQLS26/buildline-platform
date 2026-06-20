namespace Buildline.Platform.Requisition.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a material reference entry.
/// </summary>
/// <param name="Sku">Business SKU assigned to the material.</param>
/// <param name="Name">Material name displayed in reference and requisition screens.</param>
/// <param name="Category">Category name used by material filters.</param>
/// <param name="Unit">Measurement unit used for stock and requisition quantities.</param>
/// <param name="Project">Project associated with the stock record.</param>
/// <param name="CurrentStock">Initial available stock quantity.</param>
/// <param name="MinStock">Minimum stock threshold.</param>
/// <param name="MaxStock">Maximum stock threshold.</param>
/// <param name="CompanyId">Company profile identifier that owns the created operational record.</param>
/// <remarks>
///     This command supports TS-05 and keeps REST resources decoupled from the domain aggregate.
/// </remarks>
public record CreateMaterialCommand(
    string Sku,
    string Name,
    string Category,
    string Unit,
    string Project,
    int CurrentStock,
    int MinStock,
    int MaxStock,
    int CompanyId = 1);



