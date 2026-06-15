using Buildline.Platform.Requisition.Domain.Model.Commands;
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
    ///     Creates a requisition aggregate from the material requisition command contract.
    /// </summary>
    /// <param name="command">Material requisition payload submitted by the resident engineer workflow.</param>
    public Requisition(CreateRequisitionCommand command)
    {
        ReqId = string.IsNullOrWhiteSpace(command.ReqId) ? $"MR-{DateTime.UtcNow:yyyyMMddHHmmss}" : command.ReqId.Trim();
        Material = command.Material?.Trim() ?? string.Empty;
        Project = command.Project?.Trim() ?? string.Empty;
        Quantity = command.Quantity ?? 0;
        Unit = command.Unit?.Trim() ?? string.Empty;
        Priority = command.Priority?.Trim() ?? "Medium";
        Status = command.Status?.Trim() ?? "Pending";
        RequestedOn = command.RequestedOn?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        DeliveryDate = command.DeliveryDate?.Trim() ?? string.Empty;
        RequestedBy = command.RequestedBy?.Trim() ?? string.Empty;
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
    /// <param name="command">Command containing only the fields that must change.</param>
    public void Apply(UpdateRequisitionCommand command)
    {
        ReqId = command.ReqId is null ? ReqId : command.ReqId.Trim();
        Material = command.Material is null ? Material : command.Material.Trim();
        Project = command.Project is null ? Project : command.Project.Trim();
        Quantity = command.Quantity ?? Quantity;
        Unit = command.Unit is null ? Unit : command.Unit.Trim();
        Priority = command.Priority is null ? Priority : command.Priority.Trim();
        Status = command.Status is null ? Status : command.Status.Trim();
        RequestedOn = command.RequestedOn is null ? RequestedOn : command.RequestedOn.Trim();
        DeliveryDate = command.DeliveryDate is null ? DeliveryDate : command.DeliveryDate.Trim();
        RequestedBy = command.RequestedBy is null ? RequestedBy : command.RequestedBy.Trim();
    }
}



