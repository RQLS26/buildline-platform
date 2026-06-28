using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Requisition.Domain.Model.Aggregates;

/// <summary>
///     Audit timestamp projection for the <see cref="Material" /> aggregate.
/// </summary>
public partial class Material : IAuditableEntity
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