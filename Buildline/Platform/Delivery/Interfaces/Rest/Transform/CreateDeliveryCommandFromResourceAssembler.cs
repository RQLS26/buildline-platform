using Buildline.Platform.Delivery.Domain.Model.Commands;
using Buildline.Platform.Delivery.Interfaces.Rest.Resources;

namespace Buildline.Platform.Delivery.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateDeliveryCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateDeliveryCommand ToCommandFromResource(DeliveryWriteResource resource)
    {
        return new CreateDeliveryCommand(
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
