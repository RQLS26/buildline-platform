using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Procurement.Domain.Model.Aggregates;

/// <summary>
///     Audit timestamp projection for the <see cref="PurchaseOrder" /> aggregate.
/// </summary>
public partial class PurchaseOrder : IAuditableEntity
{
    /// <summary>
    ///     Gets or sets the timestamp captured when the aggregate is first persisted.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp captured when the aggregate is updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}