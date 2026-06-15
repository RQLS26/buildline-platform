namespace Buildline.Platform.Categories.Domain.Model.Queries;

/// <summary>
///     Query that requests every material category available as catalog reference data.
/// </summary>
/// <remarks>
///     This query supports TS-09 and read-only frontend filters that classify materials by category.
/// </remarks>
public record GetAllCategoriesQuery;
