using System.Net.Mime;
using System.Text.Json;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Shared.Infrastructure.Pipeline.Middleware.Components;

/// <summary>
///     Middleware that converts unhandled application exceptions into localized Problem Details responses.
/// </summary>
/// <param name="next">Next middleware delegate in the ASP.NET Core request pipeline.</param>
/// <param name="logger">Logger used to record unexpected exceptions and cancelled requests.</param>
/// <param name="errorLocalizer">Localizer used to resolve bounded-context error messages.</param>
/// <param name="commonLocalizer">Localizer used to resolve shared response titles.</param>
/// <remarks>
///     Controllers and command services handle expected application failures through typed results. This
///     middleware is the last defensive boundary for unexpected failures, keeping the public REST contract
///     consistent even when an exception escapes lower layers.
/// </remarks>
public class GlobalExceptionHandlerMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionHandlerMiddleware> logger,
    IStringLocalizer<ErrorMessages> errorLocalizer,
    IStringLocalizer<CommonMessages> commonLocalizer)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    /// <summary>
    ///     Executes the middleware and delegates successful requests to the next pipeline component.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <returns>A task that completes when the request has been processed.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException ex)
        {
            logger.LogWarning(ex, "Request was cancelled for {Path}", context.Request.Path);
            await HandleOperationCanceledExceptionAsync(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred for {Path}", context.Request.Path);
            await HandleGenericExceptionAsync(context);
        }
    }

    /// <summary>
    ///     Writes a localized conflict response for cancelled operations.
    /// </summary>
    /// <param name="context">HTTP context that will receive the Problem Details payload.</param>
    /// <returns>A task that completes when the response body has been written.</returns>
    private async Task HandleOperationCanceledExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status409Conflict;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = errorLocalizer["OperationCancelled"],
            Detail = errorLocalizer["OperationCancelled"],
            Instance = context.Request.Path
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, JsonOptions));
    }

    /// <summary>
    ///     Writes a localized internal-server-error response for unexpected failures.
    /// </summary>
    /// <param name="context">HTTP context that will receive the Problem Details payload.</param>
    /// <returns>A task that completes when the response body has been written.</returns>
    private async Task HandleGenericExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = commonLocalizer["InternalServerError"],
            Detail = errorLocalizer["GenericError"],
            Instance = context.Request.Path
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, JsonOptions));
    }
}
