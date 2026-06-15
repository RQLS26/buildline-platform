using System.Net.Mime;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;
using Buildline.Platform.Iam.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/auth")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints.")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    [HttpPost("sign-in")]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "Authenticates a Buildline user and returns a JWT token.",
        OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated.", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid email or password.")]
    public async Task<IActionResult> SignIn([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromSignInResult(
            this,
            result,
            problemDetailsFactory,
            authenticatedUser => Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                authenticatedUser.user,
                authenticatedUser.token)));
    }

    [HttpPost("sign-up")]
    [SwaggerOperation(
        Summary = "Sign up",
        Description = "Registers a Buildline user and returns an authenticated session token.",
        OperationId = "SignUp")]
    [SwaggerResponse(StatusCodes.Status201Created, "The user was registered and authenticated.", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status409Conflict, "The email address is already registered.")]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken cancellationToken)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);

        return IamActionResultAssembler.ToActionResultFromSignUpResult(
            this,
            result,
            problemDetailsFactory,
            authenticatedUser => Created(
                $"/api/v1/users/{authenticatedUser.user.Id}",
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(
                    authenticatedUser.user,
                    authenticatedUser.token)));
    }
}
