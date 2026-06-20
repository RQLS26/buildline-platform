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

/// <summary>REST controller for company-scoped incidents resources.</summary>
[ApiController]
[Authorize]
[Route("api/v1/companies/{companyId:int}/incidents")]
[Route("api/v1/incidents")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Company-scoped incidents endpoints.")]
public class IncidentsController(
    ISupplierIncidentCommandService supplierIncidentCommandService,
    ISupplierIncidentQueryService incidentQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every incident owned by the route company.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllIncidents([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var items = await incidentQueryService.ListAsync(cancellationToken);
        return Ok(items.Where(item => item.CompanyId == resolvedCompanyId).Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one incident owned by the route company.</summary>
    [HttpGet("{incidentId:int}")]
    public async Task<IActionResult> GetIncidentById([FromRoute] int? companyId, int incidentId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var item = await incidentQueryService.FindByIdAsync(incidentId, cancellationToken);
        return item is null || item.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("Incident", incidentId)
            : Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(item));
    }

    /// <summary>Creates a incident owned by the route company.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateIncident([FromRoute] int? companyId, [FromBody] IncidentWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var command = CreateSupplierIncidentCommandFromResourceAssembler.ToCommandFromResource(resource, resolvedCompanyId);
        var result = await supplierIncidentCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => CreatedAtAction(nameof(GetIncidentById), new { companyId = resolvedCompanyId, incidentId = item.Id }, IncidentResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }

    /// <summary>Applies a partial update to a incident owned by the route company.</summary>
    [HttpPatch("{incidentId:int}")]
    public async Task<IActionResult> PatchIncidentById([FromRoute] int? companyId, int incidentId, [FromBody] IncidentWriteResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var existing = await incidentQueryService.FindByIdAsync(incidentId, cancellationToken);
        if (existing is null || existing.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("Incident", incidentId);

        var command = UpdateSupplierIncidentCommandFromResourceAssembler.ToCommandFromResource(incidentId, resource);
        var result = await supplierIncidentCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            item => Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(item)));
    }
}
