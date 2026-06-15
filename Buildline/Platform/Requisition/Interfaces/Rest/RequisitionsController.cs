using System.Net.Mime;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;
using Buildline.Platform.Requisition.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Requisition.Interfaces.Rest;

/// <summary>
///     REST controller for the Requisition bounded context.
/// </summary>
[ApiController]
[Authorize]
[Route("api/v1/requisitions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Material requisition endpoints for field-to-logistics workflows.")]
public class RequisitionsController(
    IRequisitionQueryService requisitionQueryService,
    IRequisitionRepository requisitionRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    ///     Gets every material requisition.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> with requisition resources.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all requisitions", OperationId = "GetAllRequisitions")]
    [SwaggerResponse(StatusCodes.Status200OK, "Requisitions were returned.", typeof(IEnumerable<RequisitionResource>))]
    public async Task<IActionResult> GetAllRequisitions(CancellationToken cancellationToken)
    {
        var requisitions = await requisitionQueryService.ListAsync(cancellationToken);
        return Ok(requisitions.Select(RequisitionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets one material requisition by identifier.
    /// </summary>
    /// <param name="requisitionId">Requisition identifier requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{requisitionId:int}")]
    [SwaggerOperation(Summary = "Get requisition by id", OperationId = "GetRequisitionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Requisition was returned.", typeof(RequisitionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Requisition was not found.")]
    public async Task<IActionResult> GetRequisitionById(int requisitionId, CancellationToken cancellationToken)
    {
        var requisition = await requisitionQueryService.FindByIdAsync(requisitionId, cancellationToken);
        return requisition is null
            ? this.NotFoundProblem("Requisition", requisitionId)
            : Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition));
    }

    /// <summary>
    ///     Creates a material requisition.
    /// </summary>
    /// <param name="resource">Material request payload submitted by the frontend form.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>201 Created</c> with the created requisition resource.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create requisition", OperationId = "CreateRequisition")]
    [SwaggerResponse(StatusCodes.Status201Created, "Requisition was created.", typeof(RequisitionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Requisition data was invalid.")]
    public async Task<IActionResult> CreateRequisition(
        [FromBody] RequisitionWriteResource resource,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Material) || string.IsNullOrWhiteSpace(resource.Project))
            return this.BadRequestProblem("Requisition", "Material and Project are required to create a requisition.");

        var requisition = new Domain.Model.Aggregates.Requisition(resource);
        await requisitionRepository.AddAsync(requisition, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetRequisitionById),
            new { requisitionId = requisition.Id },
            RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition));
    }

    /// <summary>
    ///     Applies a partial requisition update.
    /// </summary>
    /// <param name="requisitionId">Identifier of the requisition to update.</param>
    /// <param name="resource">Requisition fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>200 OK</c> with the updated requisition resource.</returns>
    [HttpPatch("{requisitionId:int}")]
    [SwaggerOperation(Summary = "Patch requisition by id", OperationId = "PatchRequisitionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Requisition was updated.", typeof(RequisitionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Requisition was not found.")]
    public async Task<IActionResult> PatchRequisitionById(
        int requisitionId,
        [FromBody] RequisitionWriteResource resource,
        CancellationToken cancellationToken)
    {
        var requisition = await requisitionQueryService.FindByIdAsync(requisitionId, cancellationToken);
        if (requisition is null) return this.NotFoundProblem("Requisition", requisitionId);

        requisition.Apply(resource);
        requisitionRepository.Update(requisition);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition));
    }
}
