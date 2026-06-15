using System.Net.Mime;
using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Procurement.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Procurement.Interfaces.Rest;

/// <summary>
///     REST controller for purchase order workflows in the Procurement bounded context.
/// </summary>
[ApiController]
[Authorize]
[Route("api/v1/purchaseOrders")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Purchase order endpoints for procurement approval and purchase history workflows.")]
public class PurchaseOrdersController(
    IPurchaseOrderQueryService purchaseOrderQueryService,
    IPurchaseOrderRepository purchaseOrderRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every purchase order.</summary>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> with purchase order resources.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all purchase orders", OperationId = "GetAllPurchaseOrders")]
    [SwaggerResponse(StatusCodes.Status200OK, "Purchase orders were returned.", typeof(IEnumerable<PurchaseOrderResource>))]
    public async Task<IActionResult> GetAllPurchaseOrders(CancellationToken cancellationToken)
    {
        var orders = await purchaseOrderQueryService.ListAsync(cancellationToken);
        return Ok(orders.Select(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one purchase order by identifier.</summary>
    /// <param name="purchaseOrderId">Purchase order persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{purchaseOrderId:int}")]
    [SwaggerOperation(Summary = "Get purchase order by id", OperationId = "GetPurchaseOrderById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Purchase order was returned.", typeof(PurchaseOrderResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Purchase order was not found.")]
    public async Task<IActionResult> GetPurchaseOrderById(int purchaseOrderId, CancellationToken cancellationToken)
    {
        var order = await purchaseOrderQueryService.FindByIdAsync(purchaseOrderId, cancellationToken);
        return order is null
            ? this.NotFoundProblem("Purchase order", purchaseOrderId)
            : Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order));
    }

    /// <summary>Creates a purchase order.</summary>
    /// <param name="resource">Purchase order payload submitted by procurement staff.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>201 Created</c> with the created purchase order resource.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create purchase order", OperationId = "CreatePurchaseOrder")]
    [SwaggerResponse(StatusCodes.Status201Created, "Purchase order was created.", typeof(PurchaseOrderResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Purchase order data was invalid.")]
    public async Task<IActionResult> CreatePurchaseOrder(
        [FromBody] PurchaseOrderWriteResource resource,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.SupplierName) || string.IsNullOrWhiteSpace(resource.Material))
            return this.BadRequestProblem("Purchase order", "SupplierName and Material are required.");

        var order = new PurchaseOrder(resource);
        await purchaseOrderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetPurchaseOrderById),
            new { purchaseOrderId = order.Id },
            PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order));
    }

    /// <summary>Applies a partial purchase order update.</summary>
    /// <param name="purchaseOrderId">Identifier of the purchase order to update.</param>
    /// <param name="resource">Purchase order fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>200 OK</c> with the updated purchase order resource.</returns>
    [HttpPatch("{purchaseOrderId:int}")]
    [SwaggerOperation(Summary = "Patch purchase order by id", OperationId = "PatchPurchaseOrderById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Purchase order was updated.", typeof(PurchaseOrderResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Purchase order was not found.")]
    public async Task<IActionResult> PatchPurchaseOrderById(
        int purchaseOrderId,
        [FromBody] PurchaseOrderWriteResource resource,
        CancellationToken cancellationToken)
    {
        var order = await purchaseOrderQueryService.FindByIdAsync(purchaseOrderId, cancellationToken);
        if (order is null) return this.NotFoundProblem("Purchase order", purchaseOrderId);

        order.Apply(resource);
        purchaseOrderRepository.Update(order);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(PurchaseOrderResourceFromEntityAssembler.ToResourceFromEntity(order));
    }
}
