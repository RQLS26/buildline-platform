using System.Net.Mime;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>REST controller for company-scoped purchaseOrders resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/purchaseOrders")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped purchaseOrders endpoints.")]
public class PurchaseOrdersController(
    IPurchaseOrderCommandService purchaseOrderCommandService,
    IPurchaseOrderQueryService purchaseOrderQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every purchase order owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllPurchaseOrders([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await purchaseOrderQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one purchase order owned by the route company.</summary>
    [HttpGet("{purchaseOrderId:int}")]
    public async Task<IActionResult> GetPurchaseorderById([FromRoute] int? companyId, int purchaseOrderId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await purchaseOrderQueryService.FindByIdAsync(purchaseOrderId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Purchase order", purchaseOrderId)
            : Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a purchase order owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseorder([FromRoute] int? companyId, [FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetPurchaseorderById), new { companyId = resolvedCompanyId, purchaseOrderId = item.Id }, PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a purchase order owned by the route company.</summary>
    [HttpPatch("{purchaseOrderId:int}")]
    public async Task<IActionResult> PatchPurchaseorderById([FromRoute] int? companyId, int purchaseOrderId, [FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await purchaseOrderQueryService.FindByIdAsync(purchaseOrderId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Purchase order", purchaseOrderId);

        var command = UpdatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(purchaseOrderId, resource);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
