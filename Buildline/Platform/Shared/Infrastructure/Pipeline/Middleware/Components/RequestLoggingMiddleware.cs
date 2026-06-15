namespace Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Middleware that logs the observable result of every HTTP request handled by the API.
/// </summary>
/// <param name="next">Next middleware delegate in the ASP.NET Core request pipeline.</param>
/// <param name="logger">Logger used to write request completion diagnostics.</param>
/// <remarks>
///     The middleware intentionally logs method, path, status code and elapsed time only. It avoids request
///     bodies and authorization values so local diagnostics remain useful without leaking sensitive data.
/// </remarks>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    /// <summary>
    ///     Measures and logs the current request after downstream middleware and controllers complete.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <returns>A task that completes when the request has been processed and logged.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var startedAt = DateTimeOffset.UtcNow;
        await next(context);
        var elapsedMilliseconds = (DateTimeOffset.UtcNow - startedAt).TotalMilliseconds;

        logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds:F0} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            elapsedMilliseconds);
    }
}
