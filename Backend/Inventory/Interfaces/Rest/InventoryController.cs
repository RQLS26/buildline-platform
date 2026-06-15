using System.Net.Mime;
using Buildline.Platform.Inventory.Application.QueryServices;
using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Repositories;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;
using Buildline.Platform.Inventory.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
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
    IInventoryItemQueryService inventoryItemQueryService,
    IInventoryItemRepository inventoryItemRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>Gets every inventory item.</summary>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> with inventory item resources.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllInventoryItems(CancellationToken cancellationToken)
    {
        var items = await inventoryItemQueryService.ListAsync(cancellationToken);
        return Ok(items.Select(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one inventory item by identifier.</summary>
    /// <param name="inventoryItemId">Inventory item identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{inventoryItemId:int}")]
    public async Task<IActionResult> GetInventoryItemById(int inventoryItemId, CancellationToken cancellationToken)
    {
        var item = await inventoryItemQueryService.FindByIdAsync(inventoryItemId, cancellationToken);
        return item is null
            ? this.NotFoundProblem("Inventory item", inventoryItemId)
            : Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates an inventory item.</summary>
    /// <param name="resource">Inventory item payload.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>201 Created</c> with the created item.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateInventoryItem([FromBody] InventoryItemWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Name) || string.IsNullOrWhiteSpace(resource.Project))
            return this.BadRequestProblem("Inventory item", "Name and Project are required.");

        var item = new InventoryItem(resource);
        await inventoryItemRepository.AddAsync(item, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return CreatedAtAction(nameof(GetInventoryItemById), new { inventoryItemId = item.Id },
            InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Applies a partial inventory item update.</summary>
    /// <param name="inventoryItemId">Inventory item identifier.</param>
    /// <param name="resource">Inventory fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence.</param>
    /// <returns><c>200 OK</c> with the updated item.</returns>
    [HttpPatch("{inventoryItemId:int}")]
    public async Task<IActionResult> PatchInventoryItemById(
        int inventoryItemId,
        [FromBody] InventoryItemWriteResource resource,
        CancellationToken cancellationToken)
    {
        var item = await inventoryItemQueryService.FindByIdAsync(inventoryItemId, cancellationToken);
        if (item is null) return this.NotFoundProblem("Inventory item", inventoryItemId);

        item.Apply(resource);
        inventoryItemRepository.Update(item);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Ok(InventoryItemResourceFromEntityAssembler.ToResourceFromEntity(item));
    }
}
