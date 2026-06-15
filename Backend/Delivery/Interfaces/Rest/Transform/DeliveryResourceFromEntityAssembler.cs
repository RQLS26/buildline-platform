using Buildline.Platform.Delivery.Interfaces.Rest.Resources;

namespace Buildline.Platform.Delivery.Interfaces.Rest.Transform;

/// <summary>Assembler that maps delivery aggregates to REST resources.</summary>
public static class DeliveryResourceFromEntityAssembler
{
    /// <summary>Converts a delivery aggregate into the frontend tracking resource contract.</summary>
    /// <param name="delivery">Delivery aggregate retrieved from persistence.</param>
    /// <returns>Delivery REST resource.</returns>
    public static DeliveryResource ToResourceFromEntity(Domain.Model.Aggregates.Delivery delivery)
    {
        return new DeliveryResource(
            delivery.Id,
            delivery.TrackingId,
            delivery.PurchaseOrder,
            delivery.Supplier,
            delivery.Origin,
            delivery.Destination,
            delivery.Status,
            delivery.Eta,
            delivery.DispatchDate,
            delivery.Items);
    }
}
