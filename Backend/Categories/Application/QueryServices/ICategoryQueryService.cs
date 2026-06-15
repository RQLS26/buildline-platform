using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Categories.Domain.Model.Queries;

namespace Buildline.Platform.Categories.Application.QueryServices;

/// <summary>
///     Defines read operations exposed by the Categories application layer.
/// </summary>
/// <remarks>
///     Query services maintain the CQRS split used in the course sample: controllers translate HTTP
///     requests into queries, while repositories remain hidden behind application contracts.
/// </remarks>
public interface ICategoryQueryService
{
    /// <summary>
    ///     Handles the query that retrieves every material category.
    /// </summary>
    /// <param name="query">Query object representing the category listing request.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The categories available to material and inventory filters.</returns>
    Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves one material category by identifier.
    /// </summary>
    /// <param name="query">Query object containing the requested category id.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The category when it exists; otherwise <c>null</c>.</returns>
    Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken = default);
}
