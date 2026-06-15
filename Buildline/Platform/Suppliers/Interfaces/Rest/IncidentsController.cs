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

/// <summary>REST controller for supplier incident tracking.</summary>
[ApiController]
[Authorize]
[Route("api/v1/incidents")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Supplier incident endpoints for quality and delivery risk tracking.")]
public class IncidentsController(
    ISupplierIncidentCommandService supplierIncidentCommandService,
    ISupplierIncidentQueryService incidentQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets every supplier incident.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAllIncidents(CancellationToken cancellationToken)
    {
        var incidents = await incidentQueryService.ListAsync(cancellationToken);
        return Ok(incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets one supplier incident by identifier.</summary>
    [HttpGet("{incidentId:int}")]
    public async Task<IActionResult> GetIncidentById(int incidentId, CancellationToken cancellationToken)
    {
        var incident = await incidentQueryService.FindByIdAsync(incidentId, cancellationToken);
        return incident is null ? this.NotFoundProblem("Incident", incidentId) : Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>Creates a supplier incident through the application command service.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateIncident([FromBody] IncidentWriteResource resource, CancellationToken cancellationToken)
    {
        var command = CreateSupplierIncidentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await supplierIncidentCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => CreatedAtAction(nameof(GetIncidentById), new { incidentId = incident.Id }, IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident)));
    }

    /// <summary>Applies a partial supplier incident update through the application command service.</summary>
    [HttpPatch("{incidentId:int}")]
    public async Task<IActionResult> PatchIncidentById(int incidentId, [FromBody] IncidentWriteResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateSupplierIncidentCommandFromResourceAssembler.ToCommandFromResource(incidentId, resource);
        var result = await supplierIncidentCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            incident => Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident)));
    }
}
