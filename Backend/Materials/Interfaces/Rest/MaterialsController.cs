using System.Net.Mime;
using Buildline.Platform.Materials.Application.CommandServices;
using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Materials.Domain.Model.Queries;
using Buildline.Platform.Materials.Interfaces.Rest.Resources;
using Buildline.Platform.Materials.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Materials.Interfaces.Rest;

[ApiController]
[Route("api/v1/materials")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Material Catalog endpoints for requisitions and inventory.")]
public class MaterialsController(
    IMaterialCommandService materialCommandService,
    IMaterialQueryService materialQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
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

    [HttpGet("{materialId:int}")]
    [SwaggerOperation(
        Summary = "Get material by id",
        Description = "Gets a material from the Buildline catalog by its unique identifier.",
        OperationId = "GetMaterialById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The material was found and returned.", typeof(MaterialResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The material was not found.")]
    public async Task<IActionResult> GetMaterialById(int materialId, CancellationToken cancellationToken)
    {
        var query = new GetMaterialByIdQuery(materialId);
        var material = await materialQueryService.Handle(query, cancellationToken);

        return MaterialsActionResultAssembler.ToActionResultFromGetMaterialByIdResult(
            this,
            material,
            errorLocalizer,
            problemDetailsFactory,
            foundMaterial => Ok(MaterialResourceFromEntityAssembler.ToResourceFromEntity(foundMaterial)));
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
                nameof(GetMaterialById),
                new { materialId = createdMaterial.Id },
                MaterialResourceFromEntityAssembler.ToResourceFromEntity(createdMaterial)));
    }

    [HttpPut("{materialId:int}")]
    [SwaggerOperation(
        Summary = "Update material by id",
        Description = "Updates a material from the Buildline catalog by its unique identifier.",
        OperationId = "UpdateMaterialById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The material was updated.", typeof(MaterialResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The material was not found.")]
    public async Task<IActionResult> UpdateMaterialById(
        int materialId,
        [FromBody] UpdateMaterialResource resource,
        CancellationToken cancellationToken)
    {
        var command = UpdateMaterialCommandFromResourceAssembler.ToCommandFromResource(materialId, resource);
        var result = await materialCommandService.Handle(command, cancellationToken);

        return MaterialsActionResultAssembler.ToActionResultFromUpdateMaterialResult(
            this,
            result,
            problemDetailsFactory,
            updatedMaterial => Ok(MaterialResourceFromEntityAssembler.ToResourceFromEntity(updatedMaterial)));
    }

    [HttpPatch("{materialId:int}")]
    [SwaggerOperation(
        Summary = "Patch material by id",
        Description = "Partially-compatible update endpoint for the current inventory and material catalog frontend.",
        OperationId = "PatchMaterialById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The material was updated.", typeof(MaterialResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The material was not found.")]
    public async Task<IActionResult> PatchMaterialById(
        int materialId,
        [FromBody] UpdateMaterialResource resource,
        CancellationToken cancellationToken)
    {
        return await UpdateMaterialById(materialId, resource, cancellationToken);
    }

    [HttpDelete("{materialId:int}")]
    [SwaggerOperation(
        Summary = "Delete material by id",
        Description = "Deletes a material from the Buildline catalog by its unique identifier.",
        OperationId = "DeleteMaterialById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The material was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The material was not found.")]
    public async Task<IActionResult> DeleteMaterialById(int materialId, CancellationToken cancellationToken)
    {
        var command = new DeleteMaterialCommand(materialId);
        var result = await materialCommandService.Handle(command, cancellationToken);

        return MaterialsActionResultAssembler.ToActionResultFromDeleteMaterialResult(
            this,
            result,
            problemDetailsFactory,
            NoContent);
    }
}
