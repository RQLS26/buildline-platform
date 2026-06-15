using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateSupplierIncidentCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateSupplierIncidentCommand ToCommandFromResource(IncidentWriteResource resource)
    {
        return new CreateSupplierIncidentCommand(
            resource.IncidentId,
            resource.Title,
            resource.Description,
            resource.Supplier,
            resource.PurchaseOrder,
            resource.ReportedBy,
            resource.Severity,
            resource.Status,
            resource.Date,
            resource.Time);
    }
}
