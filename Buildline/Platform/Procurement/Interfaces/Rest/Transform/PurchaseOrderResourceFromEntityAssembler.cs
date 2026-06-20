using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that maps purchase order aggregates to REST resources.
/// </summary>
public static class PurchaseOrderResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a purchase order aggregate into the frontend purchase order resource contract.
    /// </summary>
    /// <param name="purchaseOrder">Purchase order aggregate retrieved from persistence.</param>
    /// <returns>Purchase order REST resource.</returns>
    public static PurchaseOrderResource ToResourceFromEntity(PurchaseOrder purchaseOrder)
    {
        return new PurchaseOrderResource(
            purchaseOrder.Id,
            purchaseOrder.CompanyId,
            purchaseOrder.OrderId,
            purchaseOrder.Date,
            purchaseOrder.SupplierName,
            purchaseOrder.Material,
            purchaseOrder.Project,
            purchaseOrder.TotalAmount,
            purchaseOrder.Status);
    }
}
