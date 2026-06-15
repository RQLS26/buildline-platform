using System.Net.Mime;
using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;
using Buildline.Platform.Iam.Interfaces.Rest.Transform;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Buildline.Platform.Iam.Interfaces.Rest;

/// <summary>
///     REST controller that exposes public IAM authentication endpoints.
/// </summary>
/// <param name="userCommandService">IAM command service used to authenticate or register users.</param>
/// <param name="problemDetailsFactory">Factory used to produce standardized Problem Details responses.</param>
/// <remarks>
///     This controller is intentionally anonymous because users need access to sign in and sign up
///     before they own a JWT. Protected business controllers require JWT Bearer authentication.
/// </remarks>
[ApiController]
[AllowAnonymous]
[Route("api/v1/auth")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Authentication endpoints.")]
public class AuthenticationController(
    IUserCommandService userCommandService,
    ProblemDetailsFactory problemDetailsFactory)
    : ControllerBase
{
    /// <summary>
    ///     Authenticates an existing active user and returns a JWT session resource.
    /// </summary>
    /// <param name="resource">Request body containing email and password credentials.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>200 OK</c> with authenticated user data and token when credentials are valid; otherwise
    ///     <c>401 Unauthorized</c> Problem Details.
    /// </returns>
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

    /// <summary>
    ///     Registers a new user account and returns an authenticated session resource.
    /// </summary>
    /// <param name="resource">Request body containing registration and credential fields.</param>
    /// <param name="cancellationToken">Token used to cancel the command when the HTTP request is aborted.</param>
    /// <returns>
    ///     <c>201 Created</c> with authenticated user data and token when registration succeeds;
    ///     otherwise a Problem Details response.
    /// </returns>
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
