namespace Buildline.Platform.Procurement.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by purchase order create and partial update endpoints.
/// </summary>
/// <param name="OrderId">Business purchase order code.</param>
/// <param name="Date">Display date.</param>
/// <param name="SupplierName">Supplier display name.</param>
/// <param name="Material">Purchased material description.</param>
/// <param name="Project">Project charged by the order.</param>
/// <param name="TotalAmount">Total purchase amount.</param>
/// <param name="Status">Approval workflow status.</param>
public record PurchaseOrderWriteResource(
    string? OrderId,
    string? Date,
    string? SupplierName,
    string? Material,
    string? Project,
    decimal? TotalAmount,
    string? Status);
