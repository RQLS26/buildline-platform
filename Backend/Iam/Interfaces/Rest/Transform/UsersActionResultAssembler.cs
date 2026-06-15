using Buildline.Platform.Iam.Domain.Model;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Converts IAM user-management application results into HTTP action results.
/// </summary>
/// <remarks>
///     Keeping this mapping outside the controller mirrors the learning-center sample and prevents
///     duplicated status-code decisions across users endpoints. Business errors remain expressed as
///     IAM errors in the application layer and are translated to REST Problem Details here.
/// </remarks>
public static class UsersActionResultAssembler
{
    /// <summary>
    ///     Maps IAM errors to HTTP status codes used by the users REST API.
    /// </summary>
    /// <param name="error">IAM error emitted by the application service.</param>
    /// <returns>The HTTP status code that best represents the failure.</returns>
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
    ///     Converts the users listing query result to either <c>200 OK</c> or <c>204 No Content</c>.
    /// </summary>
    /// <param name="users">Users returned by the query service.</param>
    /// <param name="successAction">Action used by the controller to shape a successful response body.</param>
    /// <returns>An HTTP action result for the listing endpoint.</returns>
    public static IActionResult ToActionResultFromGetAllUsersResult(
        IEnumerable<User> users,
        Func<IEnumerable<User>, IActionResult> successAction)
    {
        var userList = users.ToList();
        return userList.Count == 0 ? new NoContentResult() : successAction(userList);
    }

    /// <summary>
    ///     Converts a nullable user lookup result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="user">User returned by the query service, or <c>null</c> when not found.</param>
    /// <param name="errorLocalizer">Localizer used to produce the user-not-found message.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful response.</param>
    /// <returns>An HTTP action result for the user lookup endpoint.</returns>
    public static IActionResult ToActionResultFromGetUserByIdResult(
        ControllerBase controller,
        User? user,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (user is not null) return successAction(user);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromIamError(IamError.UserNotFound),
            IamError.UserNotFound,
            errorLocalizer[$"{nameof(IamError)}.{IamError.UserNotFound}"]);
    }

    /// <summary>
    ///     Converts a create-user command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the user command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the created response.</param>
    /// <returns>An HTTP action result for the create-user endpoint.</returns>
    public static IActionResult ToActionResultFromCreateUserResult(
        ControllerBase controller,
        Result<User> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Converts an update-user command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned after attempting the account metadata update.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the updated response.</param>
    /// <returns>An HTTP action result for the patch-user endpoint.</returns>
    public static IActionResult ToActionResultFromUpdateUserResult(
        ControllerBase controller,
        Result<User> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<User, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
