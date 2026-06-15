using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Model.Queries;
using Buildline.Platform.Inventory.Domain.Repositories;

namespace Buildline.Platform.Inventory.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates read access to material categories.
/// </summary>
/// <param name="categoryRepository">Repository used to retrieve persisted category aggregates.</param>
/// <remarks>
///     Categories are read-only reference data in the current Sprint 3 backend. This service keeps
///     the controller aligned with the CQRS style and prepares the context for future filtering rules.
/// </remarks>
public class CategoryQueryService(ICategoryRepository categoryRepository) : ICategoryQueryService
{
    /// <summary>
    ///     Retrieves every material category available in material references.
    /// </summary>
    /// <param name="query">Category listing query.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A collection of category aggregates, possibly empty.</returns>
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.ListAsync(cancellationToken);
    }

    /// <summary>
    ///     Retrieves a single material category by identifier.
    /// </summary>
    /// <param name="query">Category lookup query containing the requested id.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The category aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.FindByIdAsync(query.CategoryId, cancellationToken);
    }
}


