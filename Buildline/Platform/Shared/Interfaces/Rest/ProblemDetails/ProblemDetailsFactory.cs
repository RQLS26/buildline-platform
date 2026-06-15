using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Resources.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;

/// <summary>
///     Factory that creates localized Problem Details responses for REST controllers.
/// </summary>
/// <remarks>
///     The factory keeps error response shape consistent across bounded contexts while still allowing
///     each application service to return its own enum-based error code.
/// </remarks>
public class ProblemDetailsFactory
{
    private readonly Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory
        _aspNetCoreProblemDetailsFactory;

    private readonly IStringLocalizer<CommonMessages> _commonLocalizer;
    private readonly IStringLocalizer<ErrorMessages> _errorLocalizer;

    /// <summary>
    ///     Initializes the localized Problem Details factory.
    /// </summary>
    /// <param name="errorLocalizer">Localizer used for bounded-context error titles.</param>
    /// <param name="commonLocalizer">Localizer used for shared generic messages.</param>
    /// <param name="aspNetCoreProblemDetailsFactory">ASP.NET Core factory used to create framework-compatible payloads.</param>
    public ProblemDetailsFactory(
        IStringLocalizer<ErrorMessages> errorLocalizer,
        IStringLocalizer<CommonMessages> commonLocalizer,
        Microsoft.AspNetCore.Mvc.Infrastructure.ProblemDetailsFactory
            aspNetCoreProblemDetailsFactory)
    {
        _errorLocalizer = errorLocalizer;
        _commonLocalizer = commonLocalizer;
        _aspNetCoreProblemDetailsFactory = aspNetCoreProblemDetailsFactory;
    }

    /// <summary>
    ///     Creates a Problem Details response from a bounded-context enum error.
    /// </summary>
    /// <param name="controller">Controller that owns the current HTTP context.</param>
    /// <param name="statusCode">HTTP status code to return.</param>
    /// <param name="errorEnum">Typed bounded-context error value.</param>
    /// <param name="detailMessage">Localized detail message produced by the application service.</param>
    /// <returns>An action result containing a localized Problem Details payload.</returns>
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        Enum? errorEnum,
        string detailMessage)
    {
        var problemDetails = _aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            errorEnum != null ? _errorLocalizer[$"{errorEnum}"] : _commonLocalizer["GenericError"],
            detail: detailMessage
        );

        if (problemDetails == null)
        {
            problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = statusCode,
                Title = errorEnum != null ? _errorLocalizer[$"{errorEnum}"] : _commonLocalizer["GenericError"],
                Detail = detailMessage,
                Instance = controller.HttpContext.Request.Path
            };
        }
        else
        {
            problemDetails.Title =
                errorEnum != null ? _errorLocalizer[$"{errorEnum}"] : _commonLocalizer["GenericError"];
            problemDetails.Detail = detailMessage;
            problemDetails.Instance = controller.HttpContext.Request.Path;
        }

        return controller.StatusCode(statusCode, problemDetails);
    }

    /// <summary>
    ///     Creates a Problem Details response from localized title and detail keys.
    /// </summary>
    /// <param name="controller">Controller that owns the current HTTP context.</param>
    /// <param name="statusCode">HTTP status code to return.</param>
    /// <param name="titleKey">Shared localization key used for the Problem Details title.</param>
    /// <param name="detailKey">Error localization key used for the Problem Details detail.</param>
    /// <param name="detailArgs">Arguments applied to the localized detail message.</param>
    /// <returns>An action result containing a localized Problem Details payload.</returns>
    public IActionResult CreateProblemDetails(
        ControllerBase controller,
        int statusCode,
        string titleKey,
        string detailKey,
        params object[] detailArgs)
    {
        var problemDetails = _aspNetCoreProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext,
            statusCode,
            _commonLocalizer[titleKey],
            detail: _errorLocalizer[detailKey, detailArgs],
            instance: controller.HttpContext.Request.Path
        );
        return controller.StatusCode(statusCode, problemDetails);
    }
}
