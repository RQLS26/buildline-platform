using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdatePurchaseOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="purchaseOrderId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdatePurchaseOrderCommand ToCommandFromResource(int purchaseOrderId, PurchaseOrderWriteResource resource)
    {
        return new UpdatePurchaseOrderCommand(
            purchaseOrderId,
            resource.OrderId,
            resource.Date,
            resource.SupplierName,
            resource.Material,
            resource.Project,
            resource.TotalAmount,
            resource.Status);
    }
}
