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
///     REST controller for supplier incident tracking.
/// </summary>
[ApiController]
[Authorize]
[Route("api/v1/incidents")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Supplier incident endpoints for quality and delivery risk tracking.")]
public class IncidentsController(
    ISupplierIncidentQueryService incidentQueryService,
    ISupplierIncidentRepository incidentRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    ///     Gets every supplier incident.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> with incident resources.</returns>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all incidents", OperationId = "GetAllIncidents")]
    [SwaggerResponse(StatusCodes.Status200OK, "Incidents were returned.", typeof(IEnumerable<IncidentResource>))]
    public async Task<IActionResult> GetAllIncidents(CancellationToken cancellationToken)
    {
        var incidents = await incidentQueryService.ListAsync(cancellationToken);
        return Ok(incidents.Select(IncidentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets one supplier incident by identifier.
    /// </summary>
    /// <param name="incidentId">Incident persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the request is aborted.</param>
    /// <returns><c>200 OK</c> when found; otherwise <c>404 Not Found</c>.</returns>
    [HttpGet("{incidentId:int}")]
    [SwaggerOperation(Summary = "Get incident by id", OperationId = "GetIncidentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Incident was returned.", typeof(IncidentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Incident was not found.")]
    public async Task<IActionResult> GetIncidentById(int incidentId, CancellationToken cancellationToken)
    {
        var incident = await incidentQueryService.FindByIdAsync(incidentId, cancellationToken);
        return incident is null
            ? this.NotFoundProblem("Incident", incidentId)
            : Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    ///     Creates a supplier incident.
    /// </summary>
    /// <param name="resource">Incident payload from the frontend incident form.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>201 Created</c> with the created incident resource.</returns>
    [HttpPost]
    [SwaggerOperation(Summary = "Create incident", OperationId = "CreateIncident")]
    [SwaggerResponse(StatusCodes.Status201Created, "Incident was created.", typeof(IncidentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Incident data was invalid.")]
    public async Task<IActionResult> CreateIncident([FromBody] IncidentWriteResource resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(resource.Title))
            return this.BadRequestProblem("Incident", "Title is required to register a supplier incident.");

        var incident = new SupplierIncident(resource);
        await incidentRepository.AddAsync(incident, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetIncidentById),
            new { incidentId = incident.Id },
            IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident));
    }

    /// <summary>
    ///     Applies a partial incident update.
    /// </summary>
    /// <param name="incidentId">Identifier of the incident to update.</param>
    /// <param name="resource">Incident fields to replace.</param>
    /// <param name="cancellationToken">Token used to cancel persistence when the request is aborted.</param>
    /// <returns><c>200 OK</c> with the updated incident resource.</returns>
    [HttpPatch("{incidentId:int}")]
    [SwaggerOperation(Summary = "Patch incident by id", OperationId = "PatchIncidentById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Incident was updated.", typeof(IncidentResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Incident was not found.")]
    public async Task<IActionResult> PatchIncidentById(
        int incidentId,
        [FromBody] IncidentWriteResource resource,
        CancellationToken cancellationToken)
    {
        var incident = await incidentQueryService.FindByIdAsync(incidentId, cancellationToken);
        if (incident is null) return this.NotFoundProblem("Incident", incidentId);

        incident.Apply(resource);
        incidentRepository.Update(incident);
        await unitOfWork.CompleteAsync(cancellationToken);

        return Ok(IncidentResourceFromEntityAssembler.ToResourceFromEntity(incident));
    }
}
