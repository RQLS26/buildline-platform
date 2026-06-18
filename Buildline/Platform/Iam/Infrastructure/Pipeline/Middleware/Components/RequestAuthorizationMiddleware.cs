using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using Buildline.Platform.Iam.Application.QueryServices;
using Microsoft.AspNetCore.Authorization;

namespace Buildline.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     IAM middleware that validates bearer tokens for endpoints marked with authorization metadata.
/// </summary>
/// <param name="next">Next middleware delegate in the ASP.NET Core request pipeline.</param>
/// <remarks>
///     Learning Center uses an IAM-specific authorization middleware. Buildline keeps ASP.NET Core JWT
///     Bearer authentication enabled, but also exposes this IAM pipeline component so authenticated requests
///     can resolve the current user aggregate into <c>HttpContext.Items["User"]</c> for application code that
///     needs the domain user instead of raw claims.
/// </remarks>
public class RequestAuthorizationMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     Validates the current request token when the selected endpoint requires authorization.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <param name="userQueryService">IAM query service used to load the authenticated user aggregate.</param>
    /// <param name="tokenService">Token service used to validate and decode the bearer token.</param>
    /// <returns>A task that completes when authorization and downstream middleware processing finish.</returns>
    public async Task InvokeAsync(
        HttpContext context,
        IUserQueryService userQueryService,
        ITokenService tokenService)
    {
        var endpoint = context.GetEndpoint();
        var allowsAnonymous = endpoint?.Metadata.GetMetadata<IAllowAnonymous>() is not null;
        var requiresAuthorization = endpoint?.Metadata.GetOrderedMetadata<IAuthorizeData>().Any() == true;

        if (allowsAnonymous || !requiresAuthorization)
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
        if (string.IsNullOrWhiteSpace(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var userId = await tokenService.ValidateToken(token);
        if (userId is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var user = await userQueryService.FindByIdAsync(userId.Value, context.RequestAborted);
        if (user is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.Items["User"] = user;
        await next(context);
    }
}
