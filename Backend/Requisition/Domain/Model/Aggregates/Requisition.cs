using Buildline.Platform.Requisition.Interfaces.Rest.Resources;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Requisition.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a material requisition created from a construction site.
/// </summary>
/// <remarks>
///     A requisition is not a material catalog record. It captures a concrete operational need,
///     including project, quantity, requested delivery date, requester and workflow status. This keeps
///     the Requisition bounded context aligned with the Chapter 04 domain model instead of collapsing
///     the workflow into the Shared material catalog.
/// </remarks>
public partial class Requisition : IAuditableEntity
{
    /// <summary>
    ///     Initializes an empty requisition instance for Entity Framework Core materialization.
    /// </summary>
    protected Requisition()
    {
        ReqId = string.Empty;
        Material = string.Empty;
        Project = string.Empty;
        Unit = string.Empty;
        Priority = string.Empty;
        Status = string.Empty;
        RequestedOn = string.Empty;
        DeliveryDate = string.Empty;
        RequestedBy = string.Empty;
    }

    /// <summary>
    ///     Creates a requisition aggregate from the frontend material request contract.
    /// </summary>
    /// <param name="resource">Material requisition payload submitted by the resident engineer workflow.</param>
    public Requisition(RequisitionWriteResource resource)
    {
        ReqId = string.IsNullOrWhiteSpace(resource.ReqId) ? $"MR-{DateTime.UtcNow:yyyyMMddHHmmss}" : resource.ReqId.Trim();
        Material = resource.Material?.Trim() ?? string.Empty;
        Project = resource.Project?.Trim() ?? string.Empty;
        Quantity = resource.Quantity ?? 0;
        Unit = resource.Unit?.Trim() ?? string.Empty;
        Priority = resource.Priority?.Trim() ?? "Medium";
        Status = resource.Status?.Trim() ?? "Pending";
        RequestedOn = resource.RequestedOn?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        DeliveryDate = resource.DeliveryDate?.Trim() ?? string.Empty;
        RequestedBy = resource.RequestedBy?.Trim() ?? string.Empty;
    }

    /// <summary>Gets the database-generated requisition identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the business requisition code displayed in operations screens.</summary>
    public string ReqId { get; private set; }

    /// <summary>Gets the requested material name.</summary>
    public string Material { get; private set; }

    /// <summary>Gets the project requesting the material.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the requested quantity.</summary>
    public int Quantity { get; private set; }

    /// <summary>Gets the measurement unit for the requested quantity.</summary>
    public string Unit { get; private set; }

    /// <summary>Gets the operational priority assigned by the field team.</summary>
    public string Priority { get; private set; }

    /// <summary>Gets the approval workflow status.</summary>
    public string Status { get; private set; }

    /// <summary>Gets the display date when the requisition was created.</summary>
    public string RequestedOn { get; private set; }

    /// <summary>Gets the requested delivery date.</summary>
    public string DeliveryDate { get; private set; }

    /// <summary>Gets the employee who created the requisition.</summary>
    public string RequestedBy { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the requisition is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the requisition is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Applies a partial requisition update, including status transitions made by logistics staff.
    /// </summary>
    /// <param name="resource">Resource containing only the fields that must change.</param>
    public void Apply(RequisitionWriteResource resource)
    {
        ReqId = resource.ReqId is null ? ReqId : resource.ReqId.Trim();
        Material = resource.Material is null ? Material : resource.Material.Trim();
        Project = resource.Project is null ? Project : resource.Project.Trim();
        Quantity = resource.Quantity ?? Quantity;
        Unit = resource.Unit is null ? Unit : resource.Unit.Trim();
        Priority = resource.Priority is null ? Priority : resource.Priority.Trim();
        Status = resource.Status is null ? Status : resource.Status.Trim();
        RequestedOn = resource.RequestedOn is null ? RequestedOn : resource.RequestedOn.Trim();
        DeliveryDate = resource.DeliveryDate is null ? DeliveryDate : resource.DeliveryDate.Trim();
        RequestedBy = resource.RequestedBy is null ? RequestedBy : resource.RequestedBy.Trim();
    }
}
