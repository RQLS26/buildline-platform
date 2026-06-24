using System.Net.Mime;
using Buildline.Platform.Suppliers.Application.CommandServices;
using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;
using Buildline.Platform.Suppliers.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Suppliers.Interfaces.Rest;

/// <summary>REST controller for company-scoped suppliers resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/suppliers")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped suppliers endpoints.")]
public class SuppliersController(
    ISupplierCommandService supplierCommandService,
    ISupplierQueryService supplierQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every supplier owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSuppliers([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await supplierQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(SupplierResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one supplier owned by the route company.</summary>
    [HttpGet("{supplierId:int}")]
    public async Task<IActionResult> GetSupplierById([FromRoute] int? companyId, int supplierId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Supplier", supplierId)
            : Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a supplier owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromRoute] int? companyId, [FromBody] SupplierWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateSupplierCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await supplierCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetSupplierById), new { companyId = resolvedCompanyId, supplierId = item.Id }, SupplierResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a supplier owned by the route company.</summary>
    [HttpPatch("{supplierId:int}")]
    public async Task<IActionResult> PatchSupplierById([FromRoute] int? companyId, int supplierId, [FromBody] SupplierWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Supplier", supplierId);

        var command = UpdateSupplierCommandFromResourceAssembler.ToCommandFromResource(supplierId, resource);
        var result = await supplierCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(SupplierResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Deletes a supplier owned by the route company.</summary>
    [HttpDelete("{supplierId:int}")]
    public async Task<IActionResult> DeleteSupplierById([FromRoute] int? companyId, int supplierId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await supplierQueryService.FindByIdAsync(supplierId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Supplier", supplierId);

        var result = await supplierCommandService.HandleDelete(supplierId, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory, NoContent);
    }
}
