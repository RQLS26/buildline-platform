using Buildline.Platform.Analytics.Domain.Model;
using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Analytics.Interfaces.Rest.Transform;

/// <summary>
///     Converts project query results into HTTP action results.
/// </summary>
/// <remarks>
///     Centralizing the status-code mapping follows the course reference structure and keeps controllers
///     focused on orchestrating requests instead of duplicating Problem Details creation.
/// </remarks>
public static class ProjectsActionResultAssembler
{
    /// <summary>
    ///     Maps Projects bounded-context errors to HTTP status codes.
    /// </summary>
    /// <param name="error">Projects error emitted by the application or query boundary.</param>
    /// <returns>The HTTP status code that represents the failure.</returns>
    private static int ToStatusCodeFromProjectsError(ProjectsError error)
    {
        return error switch
        {
            ProjectsError.ProjectNotFound => StatusCodes.Status404NotFound,
            ProjectsError.InvalidProjectData => StatusCodes.Status400BadRequest,
            ProjectsError.OperationCancelled => StatusCodes.Status409Conflict,
            ProjectsError.DatabaseError => StatusCodes.Status500InternalServerError,
            ProjectsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    /// <summary>
    ///     Converts the project listing result to either <c>200 OK</c> or <c>204 No Content</c>.
    /// </summary>
    /// <param name="projects">Projects returned by the query service.</param>
    /// <param name="successAction">Action used by the controller to shape a successful response body.</param>
    /// <returns>An HTTP action result for the project listing endpoint.</returns>
    public static IActionResult ToActionResultFromGetAllProjectsResult(
        IEnumerable<Project> projects,
        Func<IEnumerable<Project>, IActionResult> successAction)
    {
        var projectList = projects.ToList();
        return projectList.Count == 0 ? new NoContentResult() : successAction(projectList);
    }

    /// <summary>
    ///     Converts a nullable project lookup result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="project">Project returned by the query service, or <c>null</c> when not found.</param>
    /// <param name="errorLocalizer">Localizer used to produce the project-not-found message.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful response.</param>
    /// <returns>An HTTP action result for the project lookup endpoint.</returns>
    public static IActionResult ToActionResultFromGetProjectByIdResult(
        ControllerBase controller,
        Project? project,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Project, IActionResult> successAction)
    {
        if (project is not null) return successAction(project);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromProjectsError(ProjectsError.ProjectNotFound),
            ProjectsError.ProjectNotFound,
            errorLocalizer[$"{nameof(ProjectsError)}.{ProjectsError.ProjectNotFound}"]);
    }
}


