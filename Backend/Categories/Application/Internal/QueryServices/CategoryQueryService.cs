using Buildline.Platform.Categories.Application.QueryServices;
using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Categories.Domain.Model.Queries;
using Buildline.Platform.Categories.Domain.Repositories;

namespace Buildline.Platform.Categories.Application.Internal.QueryServices;

public class CategoryQueryService(ICategoryRepository categoryRepository) : ICategoryQueryService
{
    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.ListAsync(cancellationToken);
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await categoryRepository.FindByIdAsync(query.CategoryId, cancellationToken);
    }
}
