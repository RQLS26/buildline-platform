using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Procurement.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a purchase order emitted by the procurement workflow.
/// </summary>
/// <remarks>
///     Purchase orders formalize approved spend and become the financial source used by Analytics and
///     Budgeting. The aggregate stores supplier, project, material and approval status in the exact
///     shape currently consumed by the Vue procurement screens.
/// </remarks>
public partial class PurchaseOrder : IAuditableEntity
{
    /// <summary>Initializes an empty purchase order for Entity Framework Core materialization.</summary>
    protected PurchaseOrder()
    {
        OrderId = string.Empty;
        Date = string.Empty;
        SupplierName = string.Empty;
        Material = string.Empty;
        Project = string.Empty;
        Status = string.Empty;
    }

    /// <summary>
    ///     Creates a purchase order from the procurement frontend contract.
    /// </summary>
    /// <param name="resource">Purchase order payload submitted by procurement staff.</param>
    public PurchaseOrder(PurchaseOrderWriteResource resource)
    {
        OrderId = string.IsNullOrWhiteSpace(resource.OrderId) ? $"PO-{DateTime.UtcNow:yyyyMMddHHmmss}" : resource.OrderId.Trim();
        Date = resource.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        SupplierName = resource.SupplierName?.Trim() ?? string.Empty;
        Material = resource.Material?.Trim() ?? string.Empty;
        Project = resource.Project?.Trim() ?? string.Empty;
        TotalAmount = resource.TotalAmount ?? 0m;
        Status = resource.Status?.Trim() ?? "Pending";
    }

    /// <summary>Gets the database-generated purchase order identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the business purchase order code.</summary>
    public string OrderId { get; private set; }

    /// <summary>Gets the display date used by procurement history views.</summary>
    public string Date { get; private set; }

    /// <summary>Gets the supplier display name.</summary>
    public string SupplierName { get; private set; }

    /// <summary>Gets the purchased material description.</summary>
    public string Material { get; private set; }

    /// <summary>Gets the project charged by this purchase order.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the total purchase amount.</summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>Gets the approval status used by the approval inbox.</summary>
    public string Status { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the order is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the order is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Applies a partial purchase order update, most commonly an approval status transition.
    /// </summary>
    /// <param name="resource">Purchase order fields to replace.</param>
    public void Apply(PurchaseOrderWriteResource resource)
    {
        OrderId = resource.OrderId is null ? OrderId : resource.OrderId.Trim();
        Date = resource.Date is null ? Date : resource.Date.Trim();
        SupplierName = resource.SupplierName is null ? SupplierName : resource.SupplierName.Trim();
        Material = resource.Material is null ? Material : resource.Material.Trim();
        Project = resource.Project is null ? Project : resource.Project.Trim();
        TotalAmount = resource.TotalAmount ?? TotalAmount;
        Status = resource.Status is null ? Status : resource.Status.Trim();
    }
}
