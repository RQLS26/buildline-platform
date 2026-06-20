using System.Globalization;
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
            delivery.CompanyId,
            delivery.TrackingId,
            delivery.PurchaseOrder,
            delivery.Supplier,
            delivery.Origin,
            delivery.Destination,
            ResolveStatus(delivery),
            delivery.Eta,
            delivery.DispatchDate,
            delivery.Items);
    }

    /// <summary>
    ///     Resolves the display status using persisted dates and the current UTC date.
    /// </summary>
    /// <param name="delivery">Delivery aggregate whose tracking dates are evaluated.</param>
    /// <returns>Computed operational status shown by the frontend tracking timeline.</returns>
    /// <remarks>
    ///     The stored status remains available for manual corrections, but read models progress from
    ///     Shipped to In Transit and Delivered as calendar days pass. This keeps demo data alive
    ///     without requiring a background worker in Sprint 3.
    /// </remarks>
    private static string ResolveStatus(Domain.Model.Aggregates.Delivery delivery)
    {
        if (string.Equals(delivery.Status, "Delayed", StringComparison.OrdinalIgnoreCase)) return delivery.Status;
        var today = DateTime.UtcNow.Date;
        if (TryParseDate(delivery.Eta, out var eta) && today >= eta.Date) return "Delivered";
        if (TryParseDate(delivery.DispatchDate, out var dispatchDate))
        {
            if (today > dispatchDate.Date) return "In Transit";
            if (today == dispatchDate.Date) return "Shipped";
            return "Confirmed";
        }
        return string.IsNullOrWhiteSpace(delivery.Status) ? "Confirmed" : delivery.Status;
    }

    private static bool TryParseDate(string value, out DateTime date)
    {
        return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out date) ||
               DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out date);
    }

}
