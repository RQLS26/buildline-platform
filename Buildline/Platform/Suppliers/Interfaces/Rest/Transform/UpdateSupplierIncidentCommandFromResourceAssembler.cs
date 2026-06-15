using Buildline.Platform.Suppliers.Domain.Model.Commands;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

namespace Buildline.Platform.Suppliers.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateSupplierIncidentCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="supplierIncidentId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateSupplierIncidentCommand ToCommandFromResource(int supplierIncidentId, IncidentWriteResource resource)
    {
        return new UpdateSupplierIncidentCommand(
            supplierIncidentId,
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
