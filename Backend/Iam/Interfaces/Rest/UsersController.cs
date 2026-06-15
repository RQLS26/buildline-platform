using System.Net.Mime;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Model.Queries;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;
using Buildline.Platform.Iam.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Iam.Interfaces.Rest;

/// <summary>
///     REST controller that exposes user-management operations required by the Buildline frontend.
/// </summary>
/// <remarks>
///     These endpoints complement the IAM sign-in and sign-up endpoints. They are tracked in Sprint 3
///     as US-022/US-024 integration work because the frontend users module needs account listing,
///     creation and role/status updates even though the original TS-01..TS-12 set focused on
///     profiles, materials, categories and authentication.
/// </remarks>
[ApiController]
[Authorize]
[Route("api/v1/users")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User Management endpoints.")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /// <summary>
    ///     Gets every registered user account without exposing password hashes.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with user resources when accounts exist; otherwise <c>204 No Content</c>.
    /// </returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Gets all registered Buildline users without password hashes.",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found and returned.", typeof(IEnumerable<UserResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No users are currently registered.")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();
        var users = await userQueryService.Handle(query, cancellationToken);

        return UsersActionResultAssembler.ToActionResultFromGetAllUsersResult(
            users,
            foundUsers => Ok(foundUsers.Select(UserResourceFromEntityAssembler.ToResourceFromEntity)));
    }

    /// <summary>
    ///     Gets one registered user account by identifier.
    /// </summary>
    /// <param name="userId">Identifier of the user account requested by the frontend.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the user resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
    [HttpGet("{userId:int}")]
    [SwaggerOperation(
        Summary = "Get user by id",
        Description = "Gets a registered Buildline user by identifier.",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found and returned.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> GetUserById(int userId, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(query, cancellationToken);

        return UsersActionResultAssembler.ToActionResultFromGetUserByIdResult(
            this,
            user,
            errorLocalizer,
            problemDetailsFactory,
            foundUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(foundUser)));
    }

    /// <summary>
    ///     Creates a user from the administration module.
    /// </summary>
    /// <param name="resource">Request body containing account data and the initial plain password.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>201 Created</c> with the created user resource when successful; otherwise a Problem Details response.
    /// </returns>
    /// <remarks>
    ///     The command service hashes the password and enforces email uniqueness before persisting the
    ///     aggregate. The response intentionally uses <see cref="UserResource"/> so password data never
    ///     leaves the IAM bounded context.
    /// </remarks>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create user",
        Description = "Registers a Buildline user from the user management module.",
        OperationId = "CreateUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "The user was created.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The email address is already registered.")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserResource resource, CancellationToken cancellationToken)
    {
        var command = CreateUserCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return UsersActionResultAssembler.ToActionResultFromCreateUserResult(
            this,
            result,
            problemDetailsFactory,
            createdUser => CreatedAtAction(
                nameof(GetUserById),
                new { userId = createdUser.Id },
                UserResourceFromEntityAssembler.ToResourceFromEntity(createdUser)));
    }

    /// <summary>
    ///     Patches role, status and account metadata for an existing user.
    /// </summary>
    /// <param name="userId">Identifier of the user account that must be updated.</param>
    /// <param name="resource">Partial request body with only the fields changed by the frontend.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and update work when the request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the updated user resource when successful; otherwise a Problem Details response.
    /// </returns>
    /// <remarks>
    ///     The current user is loaded first so omitted PATCH fields can be preserved before creating
    ///     the application command. This keeps frontend-friendly partial updates out of the domain
    ///     aggregate and makes the command service responsible only for validation and persistence.
    /// </remarks>
    [HttpPatch("{userId:int}")]
    [SwaggerOperation(
        Summary = "Patch user by id",
        Description = "Updates role, status or profile fields of a registered Buildline user.",
        OperationId = "PatchUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was updated.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> PatchUserById(
        int userId,
        [FromBody] UpdateUserResource resource,
        CancellationToken cancellationToken)
    {
        var currentUser = await userQueryService.Handle(new GetUserByIdQuery(userId), cancellationToken);
        var getCurrentUserResult = UsersActionResultAssembler.ToActionResultFromGetUserByIdResult(
            this,
            currentUser,
            errorLocalizer,
            problemDetailsFactory,
            foundUser => Ok(foundUser));

        if (currentUser is null) return getCurrentUserResult;

        var command = UpdateUserCommandFromResourceAssembler.ToCommandFromResource(userId, currentUser, resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return UsersActionResultAssembler.ToActionResultFromUpdateUserResult(
            this,
            result,
            problemDetailsFactory,
            updatedUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(updatedUser)));
    }
}
