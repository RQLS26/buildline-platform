using System.Net.Mime;
using Buildline.Platform.Delivery.Application.CommandServices;
using Buildline.Platform.Delivery.Application.QueryServices;
using Buildline.Platform.Delivery.Interfaces.Rest.Resources;
using Buildline.Platform.Delivery.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Delivery.Interfaces.Rest;

/// <summary>REST controller for company-scoped deliveries resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/deliveries")]
[Route("api/v1/deliveries")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped deliveries endpoints.")]
public class DeliveriesController(
    IDeliveryCommandService deliveryCommandService,
    IDeliveryQueryService deliveryQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every delivery owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllDeliveries([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await deliveryQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(DeliveryResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one delivery owned by the route company.</summary>
    [HttpGet("{deliveryId:int}")]
    public async Task<IActionResult> GetDeliveryById([FromRoute] int? companyId, int deliveryId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await deliveryQueryService.FindByIdAsync(deliveryId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Delivery", deliveryId)
            : Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a delivery owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateDelivery([FromRoute] int? companyId, [FromBody] DeliveryWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateDeliveryCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await deliveryCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetDeliveryById), new { companyId = resolvedCompanyId, deliveryId = item.Id }, DeliveryResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a delivery owned by the route company.</summary>
    [HttpPatch("{deliveryId:int}")]
    public async Task<IActionResult> PatchDeliveryById([FromRoute] int? companyId, int deliveryId, [FromBody] DeliveryWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await deliveryQueryService.FindByIdAsync(deliveryId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Delivery", deliveryId);

        var command = UpdateDeliveryCommandFromResourceAssembler.ToCommandFromResource(deliveryId, resource);
        var result = await deliveryCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(DeliveryResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
