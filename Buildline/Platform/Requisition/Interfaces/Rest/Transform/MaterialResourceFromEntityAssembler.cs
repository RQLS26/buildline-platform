using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;

namespace Buildline.Platform.Requisition.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts material aggregates into REST resources.
/// </summary>
/// <remarks>
///     Keeping this mapping explicit prevents controllers from exposing domain aggregates directly and
///     stabilizes the API contract consumed by requisition and inventory frontend views.
/// </remarks>
public static class MaterialResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a material aggregate to the resource returned by the Materials API.
    /// </summary>
    /// <param name="material">Material aggregate retrieved from persistence.</param>
    /// <returns>A frontend-compatible material resource.</returns>
    public static MaterialResource ToResourceFromEntity(Material material)
    {
        return new MaterialResource(
            material.Id,
            material.CompanyId,
            material.Sku,
            material.Name,
            material.Category,
            material.Unit,
            material.Project,
            material.CurrentStock,
            material.MinStock,
            material.MaxStock);
    }
}

