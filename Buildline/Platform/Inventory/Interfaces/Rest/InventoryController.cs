using System.Net.Mime;
using Buildline.Platform.Inventory.Application.CommandServices;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;
using Buildline.Platform.Inventory.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Inventory.Interfaces.Rest;

/// <summary>REST controller for company-scoped inventory resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/inventory")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped inventory endpoints.")]
public class InventoryItemsController(
    IInventoryItemCommandService inventoryItemCommandService,
    IInventoryItemQueryService inventoryItemQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every inventory item owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllInventoryItems([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await inventoryItemQueryService.ListByCompanyIdAsync(resolvedCompanyId, cancellationToken);
        return Ok(items.Select(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one inventory item owned by the route company.</summary>
    [HttpGet("{inventoryItemId:int}")]
    public async Task<IActionResult> GetInventoryitemById([FromRoute] int? companyId, int inventoryItemId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await inventoryItemQueryService.FindByIdAndCompanyIdAsync(inventoryItemId, resolvedCompanyId, cancellationToken);
        return item is null
            ? this.NotFoundProblem("Inventory item", inventoryItemId)
            : Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a inventory item owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateInventoryitem([FromRoute] int? companyId, [FromBody] InventoryItemWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateInventoryItemCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await inventoryItemCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetInventoryitemById), new { companyId = resolvedCompanyId, inventoryItemId = item.Id }, InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a inventory item owned by the route company.</summary>
    [HttpPatch("{inventoryItemId:int}")]
    public async Task<IActionResult> PatchInventoryitemById([FromRoute] int? companyId, int inventoryItemId, [FromBody] InventoryItemWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await inventoryItemQueryService.FindByIdAndCompanyIdAsync(inventoryItemId, resolvedCompanyId, cancellationToken);
        if (existing is null)
            return this.NotFoundProblem("Inventory item", inventoryItemId);

        var command = UpdateInventoryItemCommandFromResourceAssembler.ToCommandFromResource(inventoryItemId, resource);
        var result = await inventoryItemCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
