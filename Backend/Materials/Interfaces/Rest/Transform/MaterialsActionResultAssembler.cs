using Buildline.Platform.Materials.Domain.Model;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

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
}
