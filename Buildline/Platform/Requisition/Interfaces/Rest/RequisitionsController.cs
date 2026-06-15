using System.Net.Mime;
using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;
using Buildline.Platform.Requisition.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
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
    IRequisitionCommandService requisitionCommandService,
    IRequisitionQueryService requisitionQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every material requisition.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRequisitions(CancellationToken cancellationToken)
    {
        var requisitions = await requisitionQueryService.ListAsync(cancellationToken);
        return Ok(requisitions.Select(RequisitionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one material requisition by identifier.</summary>
    [HttpGet("{requisitionId:int}")]
    public async Task<IActionResult> GetRequisitionById(int requisitionId, CancellationToken cancellationToken)
    {
        var requisition = await requisitionQueryService.FindByIdAsync(requisitionId, cancellationToken);
        return requisition is null
            ? this.NotFoundProblem("Requisition", requisitionId)
            : Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition));
    }

    /// <summary>Creates a material requisition through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateRequisition([FromBody] RequisitionWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateRequisitionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await requisitionCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(
            this,
            result,
            problemDetailsFactory,
            requisition => CreatedAtAction(nameof(GetRequisitionById), new { requisitionId = requisition.Id }, RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition)));
    }

    /// <summary>Applies a partial requisition update through the application command service.</summary>
    [HttpPatch("{requisitionId:int}")]
    public async Task<IActionResult> PatchRequisitionById(int requisitionId, [FromBody] RequisitionWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateRequisitionCommandFromResourceAssembler.ToCommandFromResource(requisitionId, resource);
        var result = await requisitionCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(
            this,
            result,
            problemDetailsFactory,
            requisition => Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(requisition)));
    }
}

