using Buildline.Platform.Categories.Domain.Model;
using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Categories.Interfaces.Rest.Transform;

public static class CategoriesActionResultAssembler
{
    private static int ToStatusCodeFromCategoriesError(CategoriesError error)
    {
        return error switch
        {
            CategoriesError.CategoryNotFound => StatusCodes.Status404NotFound,
            CategoriesError.InvalidCategoryData => StatusCodes.Status400BadRequest,
            CategoriesError.OperationCancelled => StatusCodes.Status409Conflict,
            CategoriesError.DatabaseError => StatusCodes.Status500InternalServerError,
            CategoriesError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromGetAllCategoriesResult(
        IEnumerable<Category> categories,
        Func<IEnumerable<Category>, IActionResult> successAction)
    {
        var categoryList = categories.ToList();
        return categoryList.Count == 0 ? new NoContentResult() : successAction(categoryList);
    }

    public static IActionResult ToActionResultFromGetCategoryByIdResult(
        ControllerBase controller,
        Category? category,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Category, IActionResult> successAction)
    {
        if (category is not null) return successAction(category);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromCategoriesError(CategoriesError.CategoryNotFound),
            CategoriesError.CategoryNotFound,
            errorLocalizer[$"{nameof(CategoriesError)}.{CategoriesError.CategoryNotFound}"]);
    }
}
