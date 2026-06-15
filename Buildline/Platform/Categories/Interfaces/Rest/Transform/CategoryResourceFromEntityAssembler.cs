using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Categories.Interfaces.Rest.Resources;

namespace Buildline.Platform.Categories.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts category aggregates into REST resources.
/// </summary>
/// <remarks>
///     Keeping the assembler explicit avoids returning domain aggregates directly from controllers
///     and preserves the REST contract if the internal category model changes later.
/// </remarks>
public static class CategoryResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a category aggregate to the resource returned by the Categories API.
    /// </summary>
    /// <param name="category">Category aggregate retrieved from persistence.</param>
    /// <returns>A frontend-compatible category resource.</returns>
    public static CategoryResource ToResourceFromEntity(Category category)
    {
        return new CategoryResource(category.Id, category.Name, category.Description);
    }
}
