using System.Net.Mime;
using Buildline.Platform.Inventory.Application.CommandServices;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;
using Buildline.Platform.Inventory.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Inventory.Interfaces.Rest;

/// <summary>REST controller for the Inventory bounded context.</summary>
[ApiController]
[Authorize]
[Route("api/v1/inventory")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Inventory endpoints for stock control and low-stock dashboard workflows.")]
public class InventoryController(
    IInventoryItemCommandService inventoryItemCommandService,
    IInventoryItemQueryService inventoryItemQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every inventory item.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllInventoryItems(CancellationToken cancellationToken)
    {
        var items = await inventoryItemQueryService.ListAsync(cancellationToken);
        return Ok(items.Select(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one inventory item by identifier.</summary>
    [HttpGet("{inventoryItemId:int}")]
    public async Task<IActionResult> GetInventoryItemById(int inventoryItemId, CancellationToken cancellationToken)
    {
        var item = await inventoryItemQueryService.FindByIdAsync(inventoryItemId, cancellationToken);
        return item is null ? this.NotFoundProblem("Inventory item", inventoryItemId) : Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates an inventory item through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItemWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateInventoryItemCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await inventoryItemCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetInventoryItemById), new { inventoryItemId = item.Id }, InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial inventory item update through the application command service.</summary>
    [HttpPatch("{inventoryItemId:int}")]
    public async Task<IActionResult> PatchInventoryItemById(int inventoryItemId, [FromBody] InventoryItemWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateInventoryItemCommandFromResourceAssembler.ToCommandFromResource(inventoryItemId, resource);
        var result = await inventoryItemCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
