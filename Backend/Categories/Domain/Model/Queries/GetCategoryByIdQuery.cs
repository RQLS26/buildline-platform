namespace Buildline.Platform.Categories.Domain.Model.Queries;

/// <summary>
///     Query that requests one material category by identifier.
/// </summary>
/// <param name="CategoryId">Identifier of the category requested by the API client.</param>
/// <remarks>
///     This query supports TS-10 and keeps the category lookup use case explicit in the application
///     layer instead of leaking route parameters directly into repositories.
/// </remarks>
public record GetCategoryByIdQuery(int CategoryId);
