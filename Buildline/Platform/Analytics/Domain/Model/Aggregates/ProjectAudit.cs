using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Analytics.Domain.Model.Aggregates;

/// <summary>
///     Audit timestamp projection for the <see cref="Project" /> aggregate.
/// </summary>
public partial class Project : IAuditableEntity
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