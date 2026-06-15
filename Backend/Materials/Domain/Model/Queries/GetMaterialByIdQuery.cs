namespace Buildline.Platform.Materials.Domain.Model.Queries;

/// <summary>
///     Query that requests one material by identifier.
/// </summary>
/// <param name="MaterialId">Identifier of the material requested by the API client.</param>
/// <remarks>
///     This query supports TS-06 and keeps route parameters out of repository contracts.
/// </remarks>
public record GetMaterialByIdQuery(int MaterialId);
