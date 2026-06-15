using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateSupplierCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateSupplierCommand ToCommandFromResource(SupplierWriteResource resource)
    {
        return new CreateSupplierCommand(
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
