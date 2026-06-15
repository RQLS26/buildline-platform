using System.Net.Mime;
using Buildline.Platform.Materials.Application.CommandServices;
using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Model.Queries;
using Buildline.Platform.Materials.Interfaces.Rest.Resources;
using Buildline.Platform.Materials.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Materials.Interfaces.Rest;

[ApiController]
[Route("api/v1/materials")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Material Catalog endpoints for requisitions and inventory.")]
public class MaterialsController(
    IMaterialCommandService materialCommandService,
    IMaterialQueryService materialQueryService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all materials",
        Description = "Gets all registered materials available for requisitions and inventory workflows.",
        OperationId = "GetAllMaterials")]
    [SwaggerResponse(StatusCodes.Status200OK, "The materials were found and returned.", typeof(IEnumerable<MaterialResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No materials are currently registered.")]
    public async Task<IActionResult> GetAllMaterials(CancellationToken cancellationToken)
    {
        var query = new GetAllMaterialsQuery();
        var materials = await materialQueryService.Handle(query, cancellationToken);

        return MaterialsActionResultAssembler.ToActionResultFromGetAllMaterialsResult(
            materials,
            foundMaterials => Ok(foundMaterials.Select(MaterialResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create material",
        Description = "Registers a material in the Buildline catalog.",
        OperationId = "CreateMaterial")]
    [SwaggerResponse(StatusCodes.Status201Created, "The material was created.", typeof(MaterialResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The material data was invalid.")]
    public async Task<IActionResult> CreateMaterial(
        [FromBody] CreateMaterialResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateMaterialCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await materialCommandService.Handle(command, cancellationToken);

        return MaterialsActionResultAssembler.ToActionResultFromCreateMaterialResult(
            this,
            result,
            problemDetailsFactory,
            createdMaterial => CreatedAtAction(
                nameof(GetAllMaterials),
                new { materialId = createdMaterial.Id },
                MaterialResourceFromEntityAssembler.ToResourceFromEntity(createdMaterial)));
    }
}
