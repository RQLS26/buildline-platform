using Microsoft.AspNetCore.Mvc;

namespace Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;

/// <summary>
///     Extension helpers used by lightweight frontend-contract controllers to emit consistent REST errors.
/// </summary>
/// <remarks>
///     Some Sprint 3 endpoints expose contract-compatible CRUD operations before their full business
///     workflows are promoted to command services. These helpers keep those controllers from returning
///     ad-hoc anonymous errors while preserving the standard RFC 7807 Problem Details response shape.
/// </remarks>
public static class RestProblemDetailsExtensions
{
    /// <summary>
    ///     Creates a standardized <c>404 Not Found</c> response for a missing aggregate.
    /// </summary>
    /// <param name="controller">Controller that owns the current HTTP context.</param>
    /// <param name="resourceName">Human-readable resource name, such as Supplier or Requisition.</param>
    /// <param name="resourceId">Identifier requested by the API client.</param>
    /// <returns>An action result containing a Problem Details payload.</returns>
    public static IActionResult NotFoundProblem(this ControllerBase controller, string resourceName, int resourceId)
    {
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = $"{resourceName} not found",
            Detail = $"{resourceName} with id {resourceId} was not found.",
            Instance = controller.HttpContext.Request.Path
        };

        return controller.NotFound(problemDetails);
    }

    /// <summary>
    ///     Creates a standardized <c>400 Bad Request</c> response for invalid frontend payloads.
    /// </summary>
    /// <param name="controller">Controller that owns the current HTTP context.</param>
    /// <param name="resourceName">Human-readable resource name associated with the invalid payload.</param>
    /// <param name="detail">Detailed validation message suitable for API clients.</param>
    /// <returns>An action result containing a Problem Details payload.</returns>
    public static IActionResult BadRequestProblem(this ControllerBase controller, string resourceName, string detail)
    {
        var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = $"Invalid {resourceName} data",
            Detail = detail,
            Instance = controller.HttpContext.Request.Path
        };

        return controller.BadRequest(problemDetails);
    }
}
