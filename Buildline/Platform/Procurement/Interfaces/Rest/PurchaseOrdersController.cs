using System.Net.Mime;
using Buildline.Platform.Procurement.Application.CommandServices;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>REST controller for purchase order workflows in the Procurement bounded context.</summary>
[ApiController]
[Authorize]
[Route("api/v1/purchaseOrders")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Purchase order endpoints for procurement approval and purchase history workflows.")]
public class PurchaseOrdersController(
    IPurchaseOrderCommandService purchaseOrderCommandService,
    IPurchaseOrderQueryService purchaseOrderQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every purchase order.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllPurchaseOrders(CancellationToken cancellationToken)
    {
        var orders = await purchaseOrderQueryService.ListAsync(cancellationToken);
        return Ok(orders.Select(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one purchase order by identifier.</summary>
    [HttpGet("{purchaseOrderId:int}")]
    public async Task<IActionResult> GetPurchaseOrderById(int purchaseOrderId, CancellationToken cancellationToken)
    {
        var order = await purchaseOrderQueryService.FindByIdAsync(purchaseOrderId, cancellationToken);
        return order is null ? this.NotFoundProblem("Purchase order", purchaseOrderId) : Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order));
    }

    /// <summary>Creates a purchase order through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            order => CreatedAtAction(nameof(GetPurchaseOrderById), new { purchaseOrderId = order.Id }, PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order)));
    }

    /// <summary>Applies a partial purchase order update through the application command service.</summary>
    [HttpPatch("{purchaseOrderId:int}")]
    public async Task<IActionResult> PatchPurchaseOrderById(int purchaseOrderId, [FromBody] PurchaseOrderWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdatePurchaseOrderCommandFromResourceAssembler.ToCommandFromResource(purchaseOrderId, resource);
        var result = await purchaseOrderCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            order => Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order)));
    }
}
