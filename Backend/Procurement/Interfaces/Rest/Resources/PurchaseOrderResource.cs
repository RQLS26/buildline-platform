namespace Buildline.Platform.Procurement.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by purchase order endpoints.
/// </summary>
/// <param name="Id">Persistence identifier used by the frontend.</param>
/// <param name="OrderId">Business purchase order code.</param>
/// <param name="Date">Display date.</param>
/// <param name="SupplierName">Supplier display name.</param>
/// <param name="Material">Purchased material description.</param>
/// <param name="Project">Project charged by the purchase order.</param>
/// <param name="TotalAmount">Total purchase amount.</param>
/// <param name="Status">Approval workflow status.</param>
public record PurchaseOrderResource(
    int Id,
    string OrderId,
    string Date,
    string SupplierName,
    string Material,
    string Project,
    decimal TotalAmount,
    string Status);
