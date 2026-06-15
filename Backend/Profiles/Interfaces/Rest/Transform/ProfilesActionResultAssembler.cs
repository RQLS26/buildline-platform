using Buildline.Platform.Profiles.Domain.Model;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

public static class ProfilesActionResultAssembler
{
    private static int ToStatusCodeFromProfilesError(ProfilesError error)
    {
        return error switch
        {
            ProfilesError.ProfileNotFound => StatusCodes.Status404NotFound,
            ProfilesError.InvalidProfileData => StatusCodes.Status400BadRequest,
            ProfilesError.DatabaseError => StatusCodes.Status500InternalServerError,
            ProfilesError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromGetProfileByIdResult(
        ControllerBase controller,
        Profile? profile,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Profile, IActionResult> successAction)
    {
        if (profile is not null) return successAction(profile);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromProfilesError(ProfilesError.ProfileNotFound),
            ProfilesError.ProfileNotFound,
            errorLocalizer[$"{nameof(ProfilesError)}.{ProfilesError.ProfileNotFound}"]);
    }

    public static IActionResult ToActionResultFromUpdateProfileResult(
        ControllerBase controller,
        Result<Profile> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Profile, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromProfilesError((ProfilesError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }
}
