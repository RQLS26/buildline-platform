using Buildline.Platform.Iam.Infrastructure.Pipeline.Middleware.Components;

namespace Buildline.Platform.Iam.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Extension methods for registering IAM middleware components.
/// </summary>
public static class RequestAuthorizationMiddlewareExtensions
{
    /// <summary>
    ///     Adds IAM request authorization enrichment to the ASP.NET Core pipeline.
    /// </summary>
    /// <param name="builder">Application builder that composes middleware components.</param>
    /// <returns>The same <see cref="IApplicationBuilder" /> instance so calls can be chained.</returns>
    public static IApplicationBuilder UseRequestAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestAuthorizationMiddleware>();
    }
}
