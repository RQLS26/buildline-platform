using System.Net.Mime;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;
using Buildline.Platform.Inventory.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Inventory.Interfaces.Rest;

/// <summary>
///     REST controller that exposes material categories for reference and inventory workflows.
/// </summary>
/// <remarks>
///     The controller satisfies TS-09 and TS-10 by providing read-only category endpoints aligned
///     with the Sprint 2 frontend filters and the Requisition reference data module.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/v1/categories")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Material Category endpoints.")]
public class CategoriesController(
    ICategoryQueryService categoryQueryService)
    : ControllerBase
{
    /// <summary>
    ///     Gets every material category available for reference filters.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with category resources when records exist; otherwise <c>204 No Content</c>.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all material categories",
        Description = "Gets all material categories available for reference and inventory filters.",
        OperationId = "GetAllCategories")]
    [SwaggerResponse(StatusCodes.Status200OK, "The categories were found and returned.", typeof(IEnumerable<CategoryResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No categories are currently registered.")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var categories = await categoryQueryService.ListAsync(cancellationToken);
        return Ok(categories.Select(CategoryResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets one material category by identifier.
    /// </summary>
    /// <param name="categoryId">Identifier of the category requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the category resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
    [HttpGet("{categoryId:int}")]
    [SwaggerOperation(
        Summary = "Get material category by id",
        Description = "Gets a material category by its unique identifier.",
        OperationId = "GetCategoryById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The category was found and returned.", typeof(CategoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The category was not found.")]
    public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken cancellationToken)
    {
        var category = await categoryQueryService.FindByIdAsync(categoryId, cancellationToken);
        return category is null
            ? this.NotFoundProblem("Category", categoryId)
            : Ok(CategoryResourceFromEntityAssembler.ToResourceFromEntity(category));
    }
}
