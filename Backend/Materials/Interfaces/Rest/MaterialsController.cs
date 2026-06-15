using System.Net.Mime;
using Buildline.Platform.Materials.Application.CommandServices;
using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Materials.Domain.Model.Queries;
using Buildline.Platform.Materials.Interfaces.Rest.Resources;
using Buildline.Platform.Materials.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Materials.Interfaces.Rest;

/// <summary>
///     REST controller that exposes the material catalog required by requisitions and inventory.
/// </summary>
/// <remarks>
///     The controller satisfies TS-04 through TS-08, replacing the Sprint 2 mock material service with
///     versioned .NET Web Services aligned to the current frontend fields.
/// </remarks>
[ApiController]
[Authorize]
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
    /// <summary>
    ///     Gets every registered material.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with material resources when records exist; otherwise <c>204 No Content</c>.
    /// </returns>
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

    /// <summary>
    ///     Gets one material by identifier.
    /// </summary>
    /// <param name="materialId">Identifier of the material requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the material resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
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

    /// <summary>
    ///     Creates a new material catalog record.
    /// </summary>
    /// <param name="resource">Request body containing catalog and stock fields.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>201 Created</c> with the created material resource when successful; otherwise a Problem Details response.
    /// </returns>
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

    /// <summary>
    ///     Replaces catalog and stock information for an existing material.
    /// </summary>
    /// <param name="materialId">Identifier of the material that must be updated.</param>
    /// <param name="resource">Request body containing replacement catalog and stock values.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the updated material resource when successful; otherwise a Problem Details response.
    /// </returns>
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

    /// <summary>
    ///     Provides PATCH-compatible material updates for the frontend contract.
    /// </summary>
    /// <param name="materialId">Identifier of the material that must be updated.</param>
    /// <param name="resource">Request body containing the material values sent by the frontend form.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the updated material resource when successful; otherwise a Problem Details response.
    /// </returns>
    /// <remarks>
    ///     The current frontend sends complete material payloads, so PATCH delegates to the PUT
    ///     implementation while preserving the endpoint expected by the mock-service contract.
    /// </remarks>
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

    /// <summary>
    ///     Deletes an existing material by identifier.
    /// </summary>
    /// <param name="materialId">Identifier of the material that must be removed.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>204 No Content</c> when deletion succeeds; otherwise a Problem Details response.
    /// </returns>
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
