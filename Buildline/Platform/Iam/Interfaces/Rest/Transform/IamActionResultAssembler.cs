using Buildline.Platform.Iam.Domain.Model;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Converts IAM authentication results into HTTP action results.
/// </summary>
/// <remarks>
///     The assembler centralizes status-code mapping for TS-11 and TS-12 so authentication
///     controllers stay focused on translating REST payloads into commands.
/// </remarks>
public static class IamActionResultAssembler
{
    /// <summary>
    ///     Maps IAM errors to HTTP status codes.
    /// </summary>
    /// <param name="error">IAM error emitted by the application layer.</param>
    /// <returns>The HTTP status code that represents the failure.</returns>
    private static int ToStatusCodeFromIamError(IamError error)
    {
        return error switch
        {
            IamError.UserNotFound => StatusCodes.Status404NotFound,
            IamError.EmailAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.InvalidCredentials => StatusCodes.Status401Unauthorized,
            IamError.OperationCancelled => StatusCodes.Status409Conflict,
            IamError.DatabaseError => StatusCodes.Status500InternalServerError,
            IamError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    /// <summary>
    ///     Converts a sign-in command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the IAM command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful authentication response.</param>
    /// <returns>An HTTP action result for the sign-in endpoint.</returns>
    public static IActionResult ToActionResultFromSignInResult(
        ControllerBase controller,
        Result<(User user, string token)> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<(User user, string token), IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Converts a sign-up command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the IAM command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful registration response.</param>
    /// <returns>An HTTP action result for the sign-up endpoint.</returns>
    public static IActionResult ToActionResultFromSignUpResult(
        ControllerBase controller,
        Result<(User user, string token)> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<(User user, string token), IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
