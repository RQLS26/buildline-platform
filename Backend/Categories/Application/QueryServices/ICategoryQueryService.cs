using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Categories.Domain.Model.Queries;

namespace Buildline.Platform.Categories.Application.QueryServices;

public interface ICategoryQueryService
{
    Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken = default);
    Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken = default);
}
