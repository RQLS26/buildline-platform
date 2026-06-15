using Buildline.Platform.Categories.Domain.Model;
using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Categories.Interfaces.Rest.Transform;

/// <summary>
///     Converts category query results into HTTP action results.
/// </summary>
/// <remarks>
///     Centralizing response creation mirrors the learning-center sample and prevents duplicated
///     status-code and Problem Details mapping across category endpoints.
/// </remarks>
public static class CategoriesActionResultAssembler
{
    /// <summary>
    ///     Maps category bounded-context errors to HTTP status codes.
    /// </summary>
    /// <param name="error">Category error emitted by the query boundary.</param>
    /// <returns>The HTTP status code that represents the failure.</returns>
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

    /// <summary>
    ///     Converts the category listing result to either <c>200 OK</c> or <c>204 No Content</c>.
    /// </summary>
    /// <param name="categories">Categories returned by the query service.</param>
    /// <param name="successAction">Action used by the controller to shape a successful response body.</param>
    /// <returns>An HTTP action result for the category listing endpoint.</returns>
    public static IActionResult ToActionResultFromGetAllCategoriesResult(
        IEnumerable<Category> categories,
        Func<IEnumerable<Category>, IActionResult> successAction)
    {
        var categoryList = categories.ToList();
        return categoryList.Count == 0 ? new NoContentResult() : successAction(categoryList);
    }

    /// <summary>
    ///     Converts a nullable category lookup result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="category">Category returned by the query service, or <c>null</c> when not found.</param>
    /// <param name="errorLocalizer">Localizer used to produce the category-not-found message.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful response.</param>
    /// <returns>An HTTP action result for the category lookup endpoint.</returns>
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
