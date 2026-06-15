using System.Net.Mime;
using Buildline.Platform.Delivery.Application.CommandServices;
using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Interfaces.Rest.Resources;
using Buildline.Platform.Delivery.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
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
    IDeliveryCommandService deliveryCommandService,
    IDeliveryQueryService deliveryQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every tracked delivery.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDeliveries(CancellationToken cancellationToken)
    {
        var deliveries = await deliveryQueryService.ListAsync(cancellationToken);
        return Ok(deliveries.Select(DeliveryResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one delivery by identifier.</summary>
    [HttpGet("{deliveryId:int}")]
    public async Task<IActionResult> GetDeliveryById(int deliveryId, CancellationToken cancellationToken)
    {
        var delivery = await deliveryQueryService.FindByIdAsync(deliveryId, cancellationToken);
        return delivery is null ? this.NotFoundProblem("Delivery", deliveryId) : Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery));
    }

    /// <summary>Creates a delivery tracking record through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateDelivery([FromBody] DeliveryWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateDeliveryCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await deliveryCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            delivery => CreatedAtAction(nameof(GetDeliveryById), new { deliveryId = delivery.Id }, DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery)));
    }

    /// <summary>Applies a partial delivery update through the application command service.</summary>
    [HttpPatch("{deliveryId:int}")]
    public async Task<IActionResult> PatchDeliveryById(int deliveryId, [FromBody] DeliveryWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateDeliveryCommandFromResourceAssembler.ToCommandFromResource(deliveryId, resource);
        var result = await deliveryCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            delivery => Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(delivery)));
    }
}
