using Buildline.Platform.Profiles.Domain.Model;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Converts profile query and command results into HTTP action results.
/// </summary>
/// <remarks>
///     Centralized response mapping keeps profile controllers aligned with the learning-center
///     Problem Details pattern.
/// </remarks>
public static class ProfilesActionResultAssembler
{
    /// <summary>
    ///     Maps profile bounded-context errors to HTTP status codes.
    /// </summary>
    /// <param name="error">Profile error emitted by the application layer.</param>
    /// <returns>The HTTP status code that represents the failure.</returns>
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

    /// <summary>
    ///     Converts a nullable profile lookup result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="profile">Profile returned by the query service, or <c>null</c> when not found.</param>
    /// <param name="errorLocalizer">Localizer used to produce the profile-not-found message.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful response.</param>
    /// <returns>An HTTP action result for the profile lookup endpoint.</returns>
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

    /// <summary>
    ///     Converts an update-profile command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the profile command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the updated response.</param>
    /// <returns>An HTTP action result for the profile update endpoint.</returns>
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

    /// <summary>
    ///     Converts the profile listing result to either <c>200 OK</c> or <c>204 No Content</c>.
    /// </summary>
    /// <param name="profiles">Profiles returned by the query service.</param>
    /// <param name="successAction">Action used by the controller to shape a successful response body.</param>
    /// <returns>An HTTP action result for the profile listing endpoint.</returns>
    public static IActionResult ToActionResultFromGetAllProfilesResult(
        IEnumerable<Profile> profiles,
        Func<IEnumerable<Profile>, IActionResult> successAction)
    {
        var profileList = profiles.ToList();
        return profileList.Count == 0 ? new NoContentResult() : successAction(profileList);
    }
}
