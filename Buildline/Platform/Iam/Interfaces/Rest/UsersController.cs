using System.Net.Mime;
using System.Security.Claims;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;
using Buildline.Platform.Iam.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.Company;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Buildline.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UserAggregate = Buildline.Platform.Iam.Domain.Model.Aggregates.User;

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
[Route("api/v1/companies/{companyId:int}/users")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User Management endpoints.")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /// <summary>
    ///     Gets every registered user account without exposing password hashes.
    /// </summary>
    /// <param name="companyId">Company route identifier that scopes the user collection.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with user resources when accounts exist; otherwise <c>204 No Content</c>.
    /// </returns>
    [HttpGet]
    [Authorize(Roles = "owner,admin")]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Gets all registered Buildline users without password hashes.",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found and returned.", typeof(IEnumerable<UserResource>))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "No users are currently registered.")]
    public async Task<IActionResult> GetAllUsers([FromRoute] int? companyId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var actor = await GetCurrentUserAsync(cancellationToken);
        if (actor is null || actor.CompanyId != resolvedCompanyId) return Forbid();
        var users = await userQueryService.ListAsync(cancellationToken);
        var companyUsers = users.Where(user => user.CompanyId == resolvedCompanyId);
        return Ok(companyUsers.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>
    ///     Gets the authenticated user's own IAM projection.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns><c>200 OK</c> with the authenticated user resource, or <c>403 Forbidden</c> when the token is invalid.</returns>
    [HttpGet("/api/v1/users/me")]
    [SwaggerOperation(
        Summary = "Get current user",
        Description = "Gets the authenticated user's own account and company membership state.",
        OperationId = "GetCurrentUser")]
    [SwaggerResponse(StatusCodes.Status200OK, "The current user was returned.", typeof(UserResource))]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUserAsync(cancellationToken);
        return currentUser is null ? Forbid() : Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(currentUser));
    }

    /// <summary>
    ///     Gets one registered user account by identifier.
    /// </summary>
    /// <param name="companyId">Company route identifier that scopes the requested user.</param>
    /// <param name="userId">Identifier of the user account requested by the frontend.</param>
    /// <param name="cancellationToken">Token used to cancel the query when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with the user resource when found; otherwise <c>404 Not Found</c> Problem Details.
    /// </returns>
    [HttpGet("{userId:int}")]
    [Authorize(Roles = "owner,admin")]
    [SwaggerOperation(
        Summary = "Get user by id",
        Description = "Gets a registered Buildline user by identifier.",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found and returned.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The user was not found.")]
    public async Task<IActionResult> GetUserById([FromRoute] int? companyId, int userId, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var user = await userQueryService.FindByIdAsync(userId, cancellationToken);
        return user is null || user.CompanyId != resolvedCompanyId
            ? this.NotFoundProblem("User", userId)
            : Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>
    ///     Creates a user from the administration module.
    /// </summary>
    /// <param name="companyId">Company route identifier that owns the created account.</param>
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
    [Authorize(Roles = "owner")]
    [SwaggerOperation(
        Summary = "Create user",
        Description = "Registers a Buildline user from the user management module.",
        OperationId = "CreateUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "The user was created.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The email address is already registered.")]
    public async Task<IActionResult> CreateUser([FromRoute] int? companyId, [FromBody] CreateUserResource resource, CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var actor = await GetCurrentUserAsync(cancellationToken);
        if (actor is null || actor.CompanyId != resolvedCompanyId) return Forbid();
        var scopedResource = resource with { CompanyId = resolvedCompanyId, MembershipStatus = "active" };
        var command = CreateUserCommandFromResourceAssembler.ToCommandFromResource(scopedResource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            createdUser => CreatedAtAction(
                nameof(GetUserById),
                new { companyId = resolvedCompanyId, userId = createdUser.Id },
                UserResourceFromEntityAssembler.ToResourceFromEntity(createdUser)));
    }

    /// <summary>
    ///     Patches role, status and account metadata for an existing user.
    /// </summary>
    /// <param name="companyId">Company route identifier that scopes the updated account.</param>
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
        [FromRoute] int? companyId,
        int userId,
        [FromBody] UpdateUserResource resource,
        CancellationToken cancellationToken)
    {
        if (!this.TryResolveCompanyRoute(companyId, out var resolvedCompanyId)) return Forbid();

        var currentUser = await userQueryService.FindByIdAsync(userId, cancellationToken);
        if (currentUser is null || currentUser.CompanyId != resolvedCompanyId)
            return this.NotFoundProblem("User", userId);

        var actor = await GetCurrentUserAsync(cancellationToken);
        if (actor is null || !CanPatchUser(userId, currentUser, actor, resource))
            return Forbid();

        var command = UpdateUserCommandFromResourceAssembler.ToCommandFromResource(userId, currentUser, resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            updatedUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(updatedUser)));
    }

    /// <summary>
    ///     Changes a user's password after verifying the current password.
    /// </summary>
    /// <param name="userId">Identifier of the account whose password must be changed.</param>
    /// <param name="resource">Current and new password values.</param>
    /// <param name="cancellationToken">Token used to cancel the command.</param>
    /// <returns><c>200 OK</c> with the updated user resource, or a Problem Details response.</returns>
    [HttpPatch("/api/v1/users/{userId:int}/password")]
    [SwaggerOperation(
        Summary = "Change user password",
        Description = "Changes the authenticated user's password after validating the current password.",
        OperationId = "ChangeUserPassword")]
    [SwaggerResponse(StatusCodes.Status200OK, "The password was changed.", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "The current or new password is invalid.")]
    [SwaggerResponse(StatusCodes.Status403Forbidden, "The authenticated user cannot change another user's password.")]
    public async Task<IActionResult> ChangePassword(
        int userId,
        [FromBody] ChangePasswordResource resource,
        CancellationToken cancellationToken)
    {
        if (!IsSelf(userId)) return Forbid();

        var command = new ChangePasswordCommand(userId, resource.CurrentPassword, resource.NewPassword);
        var result = await userCommandService.Handle(command, cancellationToken);
        return ApplicationResultActionResultAssembler.ToActionResult(this, result, problemDetailsFactory,
            updatedUser => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(updatedUser)));
    }

    private int? CurrentUserId => int.TryParse(User.FindFirstValue(ClaimTypes.Sid), out var id) ? id : null;

    private bool IsSelf(int userId) => CurrentUserId == userId;

    private bool IsOwner => User.IsInRole("owner");

    private async Task<UserAggregate?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        return CurrentUserId is null
            ? null
            : await userQueryService.FindByIdAsync(CurrentUserId.Value, cancellationToken);
    }

    private bool CanPatchUser(int userId, UserAggregate targetUser, UserAggregate actor, UpdateUserResource resource)
    {
        var changesRoleOrStatus = resource.Role is not null || resource.IsActive is not null;
        var changesMembership = resource.CompanyId is not null || resource.MembershipStatus is not null;
        if (targetUser.CompanyId != actor.CompanyId && !IsSelf(userId)) return false;
        if (changesMembership && (!IsOwner || IsSelf(userId))) return false;
        if (changesRoleOrStatus) return IsOwner && !IsSelf(userId);

        return IsSelf(userId) || IsOwner;
    }

}
