using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;

namespace Buildline.Platform.Requisition.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates create-material REST resources into application commands.
/// </summary>
/// <remarks>
///     The assembler keeps HTTP payload shape outside the command service and preserves the boundary
///     between the interface layer and the Materials application layer.
/// </remarks>
public static class CreateMaterialCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create-material command from the request resource.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the create-material endpoint.</param>
    /// <returns>A command ready for validation and persistence by the command service.</returns>
    public static CreateMaterialCommand ToCommandFromResource(CreateMaterialResource resource, int companyId = 1)
    {
        return new CreateMaterialCommand(
            resource.Sku,
            resource.Name,
            resource.Category,
            resource.Unit,
            resource.Project,
            resource.CurrentStock,
            resource.MinStock,
            resource.MaxStock,
            companyId);
    }
}

