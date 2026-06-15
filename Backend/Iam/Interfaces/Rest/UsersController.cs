using System.Net.Mime;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Buildline.Platform.Iam.Domain.Model.Queries;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;
using Buildline.Platform.Iam.Interfaces.Rest.Transform;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Iam.Interfaces.Rest;

[ApiController]
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
}
