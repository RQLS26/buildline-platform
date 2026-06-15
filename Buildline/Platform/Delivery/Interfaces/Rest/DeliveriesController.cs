using System.Net.Mime;
using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Domain.Repositories;
using Buildline.Platform.Delivery.Interfaces.Rest.Resources;
using Buildline.Platform.Delivery.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Delivery.Interfaces.Rest;

/// <summary>REST controller for the Delivery and Tracking bounded context.</summary>
[ApiController]
[Authorize]
[Route("api/v1/deliveries")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Delivery tracking endpoints for shipment state and ETA workflows.")]
public class DeliveriesController(
    IDeliveryQueryService deliveryQueryService,
    IDeliveryRepository deliveryRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every tracked delivery.</summary>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> with delivery resources.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllDeliveries(CancellationToken cancellationToken)
    {
        var deliveries = await deliveryQueryService.ListAsync(cancellationToken);
        return Ok(deliveries.Select(DeliveryResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one delivery by identifier.</summary>
    /// <param name="deliveryId">Delivery identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{deliveryId:int}")]
    public async Task<IActionResult> GetDeliveryById(int deliveryId, CancellationToken cancellationToken)
    {
        var delivery = await deliveryQueryService.FindByIdAsync(deliveryId, cancellationToken);
        return delivery is null
            ? this.NotFoundProblem("Delivery", deliveryId)
            : Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery));
    }

    /// <summary>Creates a delivery tracking record.</summary>
    /// <param name="resource">Delivery payload.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>201 Created</c> with the created delivery.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDelivery([FromBody] DeliveryWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.PurchaseOrder) || string.IsNullOrWhiteSpace(resource.Supplier))
            return this.BadRequestProblem("Delivery", "PurchaseOrder and Supplier are required.");

        var delivery = new Domain.Model.Aggregates.Delivery(resource);
        await deliveryRepository.AddAsync(delivery, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return CreatedAtAction(nameof(GetDeliveryById), new { deliveryId = delivery.Id },
            DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery));
    }

    /// <summary>Applies a partial delivery update.</summary>
    /// <param name="deliveryId">Delivery identifier.</param>
    /// <param name="resource">Delivery fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>200 OK</c> with the updated delivery.</returns>
    [HttpPatch("{deliveryId:int}")]
    public async Task<IActionResult> PatchDeliveryById(
        int deliveryId,
        [FromBody] DeliveryWriteResource resource,
        CancellationToken cancellationToken)
    {
        var delivery = await deliveryQueryService.FindByIdAsync(deliveryId, cancellationToken);
        if (delivery is null) return this.NotFoundProblem("Delivery", deliveryId);

        delivery.Apply(resource);
        deliveryRepository.Update(delivery);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery));
    }
}
