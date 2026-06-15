using System.Net.Mime;
using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Repositories;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;
using Buildline.Platform.Suppliers.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Suppliers.Interfaces.Rest;

/// <summary>
///     REST controller for the Suppliers bounded context.
/// </summary>
/// <remarks>
///     The endpoints replace the Sprint 2 json-server supplier directory contract while preserving
///     payload names consumed by the Vue bounded context.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/v1/suppliers")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Supplier directory endpoints for procurement risk and supplier performance workflows.")]
public class SuppliersController(
    ISupplierQueryService supplierQueryService,
    ISupplierRepository supplierRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    ///     Gets every supplier registered in the directory.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> with supplier resources.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all suppliers", OperationId = "GetAllSuppliers")]
    [SwaggerResponse(StatusCodes.Status200OK, "Suppliers were returned.", typeof(IEnumerable<SupplierResource>))]
    public async Task<IActionResult> GetAllSuppliers(CancellationToken cancellationToken)
    {
        var suppliers = await supplierQueryService.ListAsync(cancellationToken);
        return Ok(suppliers.Select(SupplierResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets a supplier by identifier.
    /// </summary>
    /// <param name="supplierId">Supplier identifier requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{supplierId:int}")]
    [SwaggerOperation(Summary = "Get supplier by id", OperationId = "GetSupplierById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Supplier was returned.", typeof(SupplierResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Supplier was not found.")]
    public async Task<IActionResult> GetSupplierById(int supplierId, CancellationToken cancellationToken)
    {
        var supplier = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        return supplier is null
            ? this.NotFoundProblem("Supplier", supplierId)
            : Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier));
    }

    /// <summary>
    ///     Creates a supplier directory entry.
    /// </summary>
    /// <param name="resource">Supplier data submitted by the frontend dialog.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>201 Created</c> with the created supplier resource.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create supplier", OperationId = "CreateSupplier")]
    [SwaggerResponse(StatusCodes.Status201Created, "Supplier was created.", typeof(SupplierResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Supplier data was invalid.")]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.CompanyName) || string.IsNullOrWhiteSpace(resource.Ruc))
            return this.BadRequestProblem("Supplier", "CompanyName and Ruc are required to register a supplier.");

        var supplier = new Supplier(resource);
        await supplierRepository.AddAsync(supplier, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetSupplierById),
            new { supplierId = supplier.Id },
            SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier));
    }

    /// <summary>
    ///     Applies a partial supplier update.
    /// </summary>
    /// <param name="supplierId">Identifier of the supplier that must be updated.</param>
    /// <param name="resource">Supplier fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>200 OK</c> with the updated supplier resource.</returns>
    [HttpPatch("{supplierId:int}")]
    [SwaggerOperation(Summary = "Patch supplier by id", OperationId = "PatchSupplierById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Supplier was updated.", typeof(SupplierResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Supplier was not found.")]
    public async Task<IActionResult> PatchSupplierById(
        int supplierId,
        [FromBody] SupplierWriteResource resource,
        CancellationToken cancellationToken)
    {
        var supplier = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        if (supplier is null) return this.NotFoundProblem("Supplier", supplierId);

        supplier.Apply(resource);
        supplierRepository.Update(supplier);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier));
    }

    /// <summary>
    ///     Deletes a supplier directory entry.
    /// </summary>
    /// <param name="supplierId">Identifier of the supplier to remove.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>204 No Content</c> when deletion succeeds.</returns>
    [HttpDelete("{supplierId:int}")]
    [SwaggerOperation(Summary = "Delete supplier by id", OperationId = "DeleteSupplierById")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Supplier was deleted.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Supplier was not found.")]
    public async Task<IActionResult> DeleteSupplierById(int supplierId, CancellationToken cancellationToken)
    {
        var supplier = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        if (supplier is null) return this.NotFoundProblem("Supplier", supplierId);

        supplierRepository.Remove(supplier);
        await unitOfWork.CompleteAsync(cancellationToken);
        return NoContent();
    }
}
