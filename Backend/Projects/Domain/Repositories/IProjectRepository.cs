using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Projects.Domain.Repositories;

/// <summary>
///     Repository contract for construction project aggregate persistence.
/// </summary>
/// <remarks>
///     The current Projects API only requires read operations inherited from
///     <see cref="IBaseRepository{TEntity}"/>. The explicit interface keeps the bounded context ready
///     for future project administration commands without leaking EF Core details upward.
/// </remarks>
public interface IProjectRepository : IBaseRepository<Project>;
