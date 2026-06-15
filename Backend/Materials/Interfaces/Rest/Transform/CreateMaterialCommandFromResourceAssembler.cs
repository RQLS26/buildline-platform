using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Materials.Interfaces.Rest.Resources;

namespace Buildline.Platform.Materials.Interfaces.Rest.Transform;

public static class CreateMaterialCommandFromResourceAssembler
{
    public static CreateMaterialCommand ToCommandFromResource(CreateMaterialResource resource)
    {
        return new CreateMaterialCommand(
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
