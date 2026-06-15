using System.Net.Mime;
using Buildline.Platform.Projects.Application.QueryServices;
using Buildline.Platform.Projects.Domain.Model.Queries;
using Buildline.Platform.Projects.Interfaces.Rest.Resources;
using Buildline.Platform.Projects.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Projects.Interfaces.Rest;

/// <summary>
///     REST controller that exposes construction projects as shared reference data.
/// </summary>
/// <remarks>
///     The endpoints satisfy the Sprint 3 Projects API planned under US-004/US-017/US-019. The data
///     is used by requisition filters, material/inventory views and analytics-budgeting dashboards.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/v1/projects")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Project reference endpoints.")]
public class ProjectsController(
    IProjectQueryService projectQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /// <summary>
    ///     Gets every construction project reference available to frontend filters.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with project resources when records exist; otherwise <c>204 No Content</c>.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all projects",
        Description = "Gets all construction projects used by requisition, inventory and analytics filters.",
        OperationId = "GetAllProjects")]
    [SwaggerResponse(StatusCodes.Status200OK, "The projects were found and returned.", typeof(IEnumerable<ProjectResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No projects are currently registered.")]
    public async Task<IActionResult> GetAllProjects(CancellationToken cancellationToken)
    {
        var query = new GetAllProjectsQuery();
        var projects = await projectQueryService.Handle(query, cancellationToken);

        return ProjectsActionResultAssembler.ToActionResultFromGetAllProjectsResult(
            projects,
            foundProjects => Ok(foundProjects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    /// <summary>
    ///     Gets one construction project reference by identifier.
    /// </summary>
    /// <param name="projectId">Identifier of the project requested by the frontend.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the project resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
    [HttpGet("{projectId:int}")]
    [SwaggerOperation(
        Summary = "Get project by id",
        Description = "Gets a construction project by its unique identifier.",
        OperationId = "GetProjectById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The project was found and returned.", typeof(ProjectResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The project was not found.")]
    public async Task<IActionResult> GetProjectById(int projectId, CancellationToken cancellationToken)
    {
        var query = new GetProjectByIdQuery(projectId);
        var project = await projectQueryService.Handle(query, cancellationToken);

        return ProjectsActionResultAssembler.ToActionResultFromGetProjectByIdResult(
            this,
            project,
            errorLocalizer,
            problemDetailsFactory,
            foundProject => Ok(ProjectResourceFromEntityAssembler.ToResourceFromEntity(foundProject)));
    }
}
