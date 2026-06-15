using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Delivery.Interfaces.Rest.Resources;

namespace Buildline.Platform.Delivery.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateDeliveryCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="deliveryId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateDeliveryCommand ToCommandFromResource(int deliveryId, DeliveryWriteResource resource)
    {
        return new UpdateDeliveryCommand(
            deliveryId,
            resource.TrackingId,
            resource.PurchaseOrder,
            resource.Supplier,
            resource.Origin,
            resource.Destination,
            resource.Status,
            resource.Eta,
            resource.DispatchDate,
            resource.Items,
            resource.Material);
    }
}
