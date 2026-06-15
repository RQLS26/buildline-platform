namespace Buildline.Platform.Materials.Domain.Model.Queries;

/// <summary>
///     Query that requests every material available in the Buildline catalog.
/// </summary>
/// <remarks>
///     This query supports TS-04 and feeds inventory/requisition views that need a complete material
///     list before applying frontend-side filters.
/// </remarks>
public record GetAllMaterialsQuery;
