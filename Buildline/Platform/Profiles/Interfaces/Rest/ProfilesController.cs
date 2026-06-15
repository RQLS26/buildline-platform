using System.Net.Mime;
using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Model.Queries;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;
using Buildline.Platform.Profiles.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Profiles.Interfaces.Rest;

/// <summary>
///     REST controller that exposes Buildline company profile operations.
/// </summary>
/// <param name="profileCommandService">Application command service used for profile updates.</param>
/// <param name="profileQueryService">Application query service used for profile lookup and listing.</param>
/// <param name="errorLocalizer">Localizer used to resolve profile error messages.</param>
/// <param name="problemDetailsFactory">Factory used to produce standardized Problem Details responses.</param>
/// <remarks>
///     The controller satisfies TS-01, TS-02 and TS-03 using versioned endpoints aligned with the
///     frontend profile settings view.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/v1/profiles")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Profile endpoints for Buildline company profile data.")]
public class ProfilesController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /// <summary>
    ///     Gets one company profile by identifier.
    /// </summary>
    /// <param name="profileId">Identifier of the profile requested by the API client.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the profile resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
    [HttpGet("{profileId:int}")]
    [SwaggerOperation(
        Summary = "Get profile by id",
        Description = "Gets the Buildline company profile by its unique identifier.",
        OperationId = "GetProfileById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was found and returned.", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The profile was not found.")]
    public async Task<IActionResult> GetProfileById(int profileId, CancellationToken cancellationToken)
    {
        var getProfileByIdQuery = new GetProfileByIdQuery(profileId);
        var profile = await profileQueryService.Handle(getProfileByIdQuery, cancellationToken);

        return ProfilesActionResultAssembler.ToActionResultFromGetProfileByIdResult(
            this,
            profile,
            errorLocalizer,
            problemDetailsFactory,
            foundProfile => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(foundProfile)));
    }

    /// <summary>
    ///     Replaces company profile information by identifier.
    /// </summary>
    /// <param name="profileId">Identifier of the profile that must be updated.</param>
    /// <param name="resource">Request body containing replacement company profile fields.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the updated profile resource when successful; otherwise a Problem Details response.
    /// </returns>
    [HttpPut("{profileId:int}")]
    [SwaggerOperation(
        Summary = "Update profile by id",
        Description = "Updates the Buildline company profile by its unique identifier.",
        OperationId = "UpdateProfileById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was updated.", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The profile was not found.")]
    public async Task<IActionResult> UpdateProfileById(
        int profileId,
        [FromBody] UpdateProfileResource resource,
        CancellationToken cancellationToken)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(profileId, resource);
        var result = await profileCommandService.Handle(updateProfileCommand, cancellationToken);

        return ProfilesActionResultAssembler.ToActionResultFromUpdateProfileResult(
            this,
            result,
            problemDetailsFactory,
            updatedProfile => Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(updatedProfile)));
    }

    /// <summary>
    ///     Provides PATCH-compatible company profile updates for the frontend form.
    /// </summary>
    /// <param name="profileId">Identifier of the profile that must be updated.</param>
    /// <param name="resource">Request body containing the profile values sent by the frontend form.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the updated profile resource when successful; otherwise a Problem Details response.
    /// </returns>
    /// <remarks>
    ///     The current frontend sends complete profile payloads, so PATCH delegates to the PUT
    ///     implementation while preserving the endpoint shape planned in TS-02.
    /// </remarks>
    [HttpPatch("{profileId:int}")]
    [SwaggerOperation(
        Summary = "Patch profile by id",
        Description = "Partially-compatible update endpoint for the current frontend profile form.",
        OperationId = "PatchProfileById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profile was updated.", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The profile was not found.")]
    public async Task<IActionResult> PatchProfileById(
        int profileId,
        [FromBody] UpdateProfileResource resource,
        CancellationToken cancellationToken)
    {
        return await UpdateProfileById(profileId, resource, cancellationToken);
    }

    /// <summary>
    ///     Gets every company profile registered in Buildline.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with profile resources when records exist; otherwise <c>204 No Content</c>.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all profiles",
        Description = "Gets the available Buildline company profiles.",
        OperationId = "GetAllProfiles")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profiles were found and returned.", typeof(IEnumerable<ProfileResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No profiles are currently registered.")]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery, cancellationToken);

        return ProfilesActionResultAssembler.ToActionResultFromGetAllProfilesResult(
            profiles,
            foundProfiles => Ok(foundProfiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity)));
    }
}
