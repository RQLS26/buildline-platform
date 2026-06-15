using Buildline.Platform.Iam.Domain.Model;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

public static class IamActionResultAssembler
{
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
