using Buildline.Platform.Inventory.Domain.Model.Commands;
using Buildline.Platform.Inventory.Interfaces.Rest.Resources;

namespace Buildline.Platform.Inventory.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateInventoryItemCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateInventoryItemCommand ToCommandFromResource(InventoryItemWriteResource resource, int companyId = 1)
    {
        return new CreateInventoryItemCommand(
            resource.Sku,
            resource.Name,
            resource.Project,
            resource.Category,
            resource.CurrentStock,
            resource.MaxStock,
            resource.MinStock,
            resource.LastUpdated,
            companyId);
    }
}
