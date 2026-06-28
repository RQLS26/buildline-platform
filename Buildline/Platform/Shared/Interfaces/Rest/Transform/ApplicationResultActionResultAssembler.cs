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
    ///     Maps bounded-context error names to REST status codes using semantic tokens instead of a
    ///     fragile single-suffix convention.
    /// </summary>
    /// <param name="error">Error enum emitted by the application layer.</param>
    /// <returns>The HTTP status code that best represents the failure.</returns>
    private static int ToStatusCodeFromError(Enum? error)
    {
        if (error is null) return StatusCodes.Status400BadRequest;

        var name = error.ToString();
        if (Contains(name, "InternalServerError") || Contains(name, "DatabaseError"))
            return StatusCodes.Status500InternalServerError;
        if (Contains(name, "OperationCancelled"))
            return 499;
        if (Contains(name, "Forbidden"))
            return StatusCodes.Status403Forbidden;
        if (Contains(name, "Unauthorized") || Contains(name, "InvalidCredentials"))
            return StatusCodes.Status401Unauthorized;
        if (Contains(name, "AlreadyTaken") || Contains(name, "AlreadyRegistered") || Contains(name, "AlreadyExists"))
            return StatusCodes.Status409Conflict;
        if (Contains(name, "NotFound"))
            return StatusCodes.Status404NotFound;
        if (Contains(name, "Invalid") || Contains(name, "WeakPassword") || Contains(name, "NotSelectable"))
            return StatusCodes.Status400BadRequest;

        return StatusCodes.Status400BadRequest;
    }

    /// <summary>
    ///     Performs ordinal, case-insensitive token matching for error names.
    /// </summary>
    /// <param name="value">Error enum name to inspect.</param>
    /// <param name="token">Semantic token expected in the enum name.</param>
    /// <returns><c>true</c> when the token appears in the value; otherwise <c>false</c>.</returns>
    private static bool Contains(string value, string token)
    {
        return value.Contains(token, StringComparison.OrdinalIgnoreCase);
    }
}
