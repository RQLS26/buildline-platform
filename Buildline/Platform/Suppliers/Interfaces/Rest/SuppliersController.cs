using System.Net.Mime;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;
using Buildline.Platform.Suppliers.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Suppliers.Interfaces.Rest;

/// <summary>REST controller for the Suppliers bounded context.</summary>
[ApiController]
[Authorize]
[Route("api/v1/suppliers")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Supplier directory endpoints for procurement risk and supplier performance workflows.")]
public class SuppliersController(
    ISupplierCommandService supplierCommandService,
    ISupplierQueryService supplierQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every supplier registered in the directory.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers(CancellationToken cancellationToken)
    {
        var suppliers = await supplierQueryService.ListAsync(cancellationToken);
        return Ok(suppliers.Select(SupplierResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a supplier by identifier.</summary>
    [HttpGet("{supplierId:int}")]
    public async Task<IActionResult> GetSupplierById(int supplierId, CancellationToken cancellationToken)
    {
        var supplier = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        return supplier is null ? this.NotFoundProblem("Supplier", supplierId) : Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier));
    }

    /// <summary>Creates a supplier directory entry through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] SupplierWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateSupplierCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await supplierCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            supplier => CreatedAtAction(nameof(GetSupplierById), new { supplierId = supplier.Id }, SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier)));
    }

    /// <summary>Applies a partial supplier update through the application command service.</summary>
    [HttpPatch("{supplierId:int}")]
    public async Task<IActionResult> PatchSupplierById(int supplierId, [FromBody] SupplierWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateSupplierCommandFromResourceAssembler.ToCommandFromResource(supplierId, resource);
        var result = await supplierCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            supplier => Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(supplier)));
    }

    /// <summary>Deletes a supplier directory entry through the application command service.</summary>
    [HttpDelete("{supplierId:int}")]
    public async Task<IActionResult> DeleteSupplierById(int supplierId, CancellationToken cancellationToken)
    {
        var result = await supplierCommandService.HandleDelete(supplierId, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, NoContent);
    }
}
