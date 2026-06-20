using System.Net.Mime;
using Buildline.Platform.Requisition.Application.CommandServices;
using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;
using Buildline.Platform.Requisition.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Requisition.Interfaces.Rest;

/// <summary>REST controller for company-scoped requisitions resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/requisitions")]
[Route("api/v1/requisitions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped requisitions endpoints.")]
public class RequisitionsController(
    IRequisitionCommandService requisitionCommandService,
    IRequisitionQueryService requisitionQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every requisition owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllRequisitions([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await requisitionQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(RequisitionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one requisition owned by the route company.</summary>
    [HttpGet("{requisitionId:int}")]
    public async Task<IActionResult> GetRequisitionById([FromRoute] int? companyId, int requisitionId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await requisitionQueryService.FindByIdAsync(requisitionId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Requisition", requisitionId)
            : Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a requisition owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateRequisition([FromRoute] int? companyId, [FromBody] RequisitionWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateRequisitionCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await requisitionCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetRequisitionById), new { companyId = resolvedCompanyId, requisitionId = item.Id }, RequisitionResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a requisition owned by the route company.</summary>
    [HttpPatch("{requisitionId:int}")]
    public async Task<IActionResult> PatchRequisitionById([FromRoute] int? companyId, int requisitionId, [FromBody] RequisitionWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await requisitionQueryService.FindByIdAsync(requisitionId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Requisition", requisitionId);

        var command = UpdateRequisitionCommandFromResourceAssembler.ToCommandFromResource(requisitionId, resource);
        var result = await requisitionCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(RequisitionResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
