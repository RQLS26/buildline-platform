using Buildline.Platform.Materials.Domain.Model;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Materials.Interfaces.Rest.Transform;

/// <summary>
///     Converts material application results into HTTP action results.
/// </summary>
/// <remarks>
///     The assembler centralizes REST decisions for TS-04 through TS-08, including empty-list
///     responses, not-found responses and command failure Problem Details.
/// </remarks>
public static class MaterialsActionResultAssembler
{
    /// <summary>
    ///     Maps material bounded-context errors to HTTP status codes.
    /// </summary>
    /// <param name="error">Material error emitted by the application layer.</param>
    /// <returns>The HTTP status code that represents the failure.</returns>
    private static int ToStatusCodeFromMaterialsError(MaterialsError error)
    {
        return error switch
        {
            MaterialsError.MaterialNotFound => StatusCodes.Status404NotFound,
            MaterialsError.InvalidMaterialData => StatusCodes.Status400BadRequest,
            MaterialsError.OperationCancelled => StatusCodes.Status409Conflict,
            MaterialsError.DatabaseError => StatusCodes.Status500InternalServerError,
            MaterialsError.InternalServerError => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };
    }

    /// <summary>
    ///     Converts the material listing result to either <c>200 OK</c> or <c>204 No Content</c>.
    /// </summary>
    /// <param name="materials">Materials returned by the query service.</param>
    /// <param name="successAction">Action used by the controller to shape a successful response body.</param>
    /// <returns>An HTTP action result for the material listing endpoint.</returns>
    public static IActionResult ToActionResultFromGetAllMaterialsResult(
        IEnumerable<Material> materials,
        Func<IEnumerable<Material>, IActionResult> successAction)
    {
        var materialList = materials.ToList();
        return materialList.Count == 0 ? new NoContentResult() : successAction(materialList);
    }

    /// <summary>
    ///     Converts a create-material command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the material command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the created response.</param>
    /// <returns>An HTTP action result for the create-material endpoint.</returns>
    public static IActionResult ToActionResultFromCreateMaterialResult(
        ControllerBase controller,
        Result<Material> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Material, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMaterialsError((MaterialsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Converts an update-material command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the material command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the updated response.</param>
    /// <returns>An HTTP action result for the update-material endpoint.</returns>
    public static IActionResult ToActionResultFromUpdateMaterialResult(
        ControllerBase controller,
        Result<Material> result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Material, IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction(result.Value!);

        var statusCode = ToStatusCodeFromMaterialsError((MaterialsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Converts a delete-material command result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="result">Application result returned by the material command service.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful no-content response.</param>
    /// <returns>An HTTP action result for the delete-material endpoint.</returns>
    public static IActionResult ToActionResultFromDeleteMaterialResult(
        ControllerBase controller,
        Result result,
        ProblemDetailsFactory problemDetailsFactory,
        Func<IActionResult> successAction)
    {
        if (result.IsSuccess) return successAction();

        var statusCode = ToStatusCodeFromMaterialsError((MaterialsError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    /// <summary>
    ///     Converts a nullable material lookup result into a REST response.
    /// </summary>
    /// <param name="controller">Controller instance used to create framework-compatible problem details.</param>
    /// <param name="material">Material returned by the query service, or <c>null</c> when not found.</param>
    /// <param name="errorLocalizer">Localizer used to produce the material-not-found message.</param>
    /// <param name="problemDetailsFactory">Factory responsible for standardized Problem Details payloads.</param>
    /// <param name="successAction">Action used by the controller to shape the successful response.</param>
    /// <returns>An HTTP action result for the material lookup endpoint.</returns>
    public static IActionResult ToActionResultFromGetMaterialByIdResult(
        ControllerBase controller,
        Material? material,
        IStringLocalizer<ErrorMessages> errorLocalizer,
        ProblemDetailsFactory problemDetailsFactory,
        Func<Material, IActionResult> successAction)
    {
        if (material is not null) return successAction(material);

        return problemDetailsFactory.CreateProblemDetails(
            controller,
            ToStatusCodeFromMaterialsError(MaterialsError.MaterialNotFound),
            MaterialsError.MaterialNotFound,
            errorLocalizer[$"{nameof(MaterialsError)}.{MaterialsError.MaterialNotFound}"]);
    }
}
