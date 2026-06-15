using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Materials.Interfaces.Rest.Resources;

namespace Buildline.Platform.Materials.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates material update REST resources into application commands.
/// </summary>
/// <remarks>
///     The route id is combined with the request body so the command service receives a single,
///     explicit command object for TS-07.
/// </remarks>
public static class UpdateMaterialCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update-material command from the route identifier and request resource.
    /// </summary>
    /// <param name="materialId">Identifier extracted from the endpoint route.</param>
    /// <param name="resource">Resource received by the update-material endpoint.</param>
    /// <returns>A command ready for validation and aggregate mutation by the command service.</returns>
    public static UpdateMaterialCommand ToCommandFromResource(int materialId, UpdateMaterialResource resource)
    {
        return new UpdateMaterialCommand(
            materialId,
            resource.Sku,
            resource.Name,
            resource.Category,
            resource.Unit,
            resource.Project,
            resource.CurrentStock,
            resource.MinStock,
            resource.MaxStock);
    }
}
