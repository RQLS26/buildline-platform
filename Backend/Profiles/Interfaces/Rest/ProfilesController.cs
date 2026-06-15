using System.Net.Mime;
using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Model.Queries;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;
using Buildline.Platform.Profiles.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Profiles.Interfaces.Rest;

[ApiController]
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

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all profiles",
        Description = "Gets the available Buildline company profiles.",
        OperationId = "GetAllProfiles")]
    [SwaggerResponse(StatusCodes.Status200OK, "The profiles were found and returned.", typeof(IEnumerable<ProfileResource>))]
    public async Task<IActionResult> GetAllProfiles(CancellationToken cancellationToken)
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery, cancellationToken);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
