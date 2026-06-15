using System.Net.Mime;
using Buildline.Platform.Categories.Application.QueryServices;
using Buildline.Platform.Categories.Domain.Model.Queries;
using Buildline.Platform.Categories.Interfaces.Rest.Resources;
using Buildline.Platform.Categories.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Categories.Interfaces.Rest;

[ApiController]
[Route("api/v1/categories")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Material Category endpoints.")]
public class CategoriesController(
    ICategoryQueryService categoryQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all material categories",
        Description = "Gets all material categories available for catalog and inventory filters.",
        OperationId = "GetAllCategories")]
    [SwaggerResponse(StatusCodes.Status200OK, "The categories were found and returned.", typeof(IEnumerable<CategoryResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No categories are currently registered.")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var categories = await categoryQueryService.Handle(query, cancellationToken);

        return CategoriesActionResultAssembler.ToActionResultFromGetAllCategoriesResult(
            categories,
            foundCategories => Ok(foundCategories.Select(CategoryResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpGet("{categoryId:int}")]
    [SwaggerOperation(
        Summary = "Get material category by id",
        Description = "Gets a material category by its unique identifier.",
        OperationId = "GetCategoryById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The category was found and returned.", typeof(CategoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The category was not found.")]
    public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(categoryId);
        var category = await categoryQueryService.Handle(query, cancellationToken);

        return CategoriesActionResultAssembler.ToActionResultFromGetCategoryByIdResult(
            this,
            category,
            errorLocalizer,
            problemDetailsFactory,
            foundCategory => Ok(CategoryResourceFromEntityAssembler.ToResourceFromEntity(foundCategory)));
    }
}
