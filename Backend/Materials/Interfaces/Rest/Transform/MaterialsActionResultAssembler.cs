using Buildline.Platform.Materials.Domain.Model;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Materials.Interfaces.Rest.Transform;

public static class MaterialsActionResultAssembler
{
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

    public static IActionResult ToActionResultFromGetAllMaterialsResult(
        IEnumerable<Material> materials,
        Func<IEnumerable<Material>, IActionResult> successAction)
    {
        var materialList = materials.ToList();
        return materialList.Count == 0 ? new NoContentResult() : successAction(materialList);
    }

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
