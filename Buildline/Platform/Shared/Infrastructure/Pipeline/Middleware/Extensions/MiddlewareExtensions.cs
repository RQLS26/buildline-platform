using Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

namespace Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Extensions;

/// <summary>
///     Extension methods used to register Buildline middleware components in the ASP.NET Core pipeline.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    ///     Adds the global exception handler middleware to the application pipeline.
    /// </summary>
    /// <param name="builder">Application builder that composes middleware components.</param>
    /// <returns>The same <see cref="IApplicationBuilder" /> instance so calls can be chained.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    /// <summary>
    ///     Adds request completion logging to the application pipeline.
    /// </summary>
    /// <param name="builder">Application builder that composes middleware components.</param>
    /// <returns>The same <see cref="IApplicationBuilder" /> instance so calls can be chained.</returns>
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
