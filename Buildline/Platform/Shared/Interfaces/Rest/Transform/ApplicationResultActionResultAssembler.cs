using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Buildline.Platform.Shared.Interfaces.Rest.Transform;

/// <summary>
///     Converts application command results into HTTP responses for bounded contexts that follow the
///     shared command-service pattern.
/// </summary>
public static class ApplicationResultActionResultAssembler
{
    /// <summary>
    ///     Converts a result containing a value into either the success response selected by the
    ///     controller or a standardized Problem Details response.
    /// </summary>
    /// <typeparam name="T">Type returned by the successful application operation.</typeparam>
    /// <param name="controller">Controller instance used to create framework-compatible Problem Details.</param>
    /// <param name="result">Application result returned by the command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized error payloads.</param>
    /// <param name="successAction">Action used to shape the successful response.</param>
    /// <returns>An HTTP action result for the application operation.</returns>
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);
        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromError(result.Error),
            result.Error,
            result.Message);
    }

    /// <summary>
    ///     Converts a value-less result into either the success response selected by the controller or
    ///     a standardized Problem Details response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible Problem Details.</param>
    /// <param name="result">Application result returned by the command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized error payloads.</param>
    /// <param name="successAction">Action used to shape the successful response.</param>
    /// <returns>An HTTP action result for the application operation.</returns>
    public static IActionResult ToActionResult(
        ControllerBase controller,
        Result result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();
        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromError(result.Error),
            result.Error,
            result.Message);
    }

    /// <summary>
    ///     Maps conventional bounded-context error names to REST status codes.
    /// </summary>
    /// <param name="error">Error enum emitted by the application layer.</param>
    /// <returns>The HTTP status code that best represents the failure.</returns>
    private static int ToStatusCodeFromError(Enum? error)
    {
        var name = error?.ToString() ?? string.Empty;
        if (name.Contains("NotFound", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status404NotFound;
        if (name.Contains("Invalid", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status400BadRequest;
        if (name.Contains("Already", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        if (name.Contains("Cancelled", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status409Conflict;
        if (name.Contains("Database", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status500InternalServerError;
        if (name.Contains("Internal", StringComparison.OrdinalIgnoreCase)) return StatusCodes.Status500InternalServerError;
        return StatusCodes.Status400BadRequest;
    }
}
