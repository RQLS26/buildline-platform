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

/// <summary>REST controller for company-scoped purchase orders resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/purchase-orders")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped purchase orders endpoints.")]
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

        var items = await purchaseOrderQueryService.ListByCompanyIdAsync(resolvedCompanyId, cancellationToken);
        return Ok(items.Select(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one purchase order owned by the route company.</summary>
    [HttpGet("{purchaseOrderId:int}")]
    public async Task<IActionResult> GetPurchaseOrderById([FromRoute] int? companyId, int purchaseOrderId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await purchaseOrderQueryService.FindByIdAndCompanyIdAsync(purchaseOrderId, resolvedCompanyId, cancellationToken);
        return item is null
            ? this.NotFoundProblem("Purchase order", purchaseOrderId)
            : Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a purchase order owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder([FromRoute] int? companyId, [FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetPurchaseOrderById), new { companyId = resolvedCompanyId, purchaseOrderId = item.Id }, PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a purchase order owned by the route company.</summary>
    [HttpPatch("{purchaseOrderId:int}")]
    public async Task<IActionResult> PatchPurchaseOrderById([FromRoute] int? companyId, int purchaseOrderId, [FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await purchaseOrderQueryService.FindByIdAndCompanyIdAsync(purchaseOrderId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Purchase order", purchaseOrderId);

        var command = UpdatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(purchaseOrderId, resource);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
