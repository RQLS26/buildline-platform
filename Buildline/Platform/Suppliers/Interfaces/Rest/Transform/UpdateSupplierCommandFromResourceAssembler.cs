using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateSupplierCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="supplierId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateSupplierCommand ToCommandFromResource(int supplierId, SupplierWriteResource resource)
    {
        return new UpdateSupplierCommand(
            supplierId,
            resource.Ruc,
            resource.CompanyName,
            resource.ContactName,
            resource.Email,
            resource.Phone,
            resource.Rating,
            resource.IsActive,
            resource.Category,
            resource.DeliveryRate);
    }
}
