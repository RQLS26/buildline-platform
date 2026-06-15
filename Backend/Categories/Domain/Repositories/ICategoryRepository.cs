using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Categories.Domain.Repositories;

/// <summary>
///     Repository contract for material category aggregate persistence.
/// </summary>
/// <remarks>
///     The Categories context currently uses read operations inherited from
///     <see cref="IBaseRepository{TEntity}"/>. The explicit interface preserves the bounded-context
///     boundary and gives future category administration commands a stable dependency point.
/// </remarks>
public interface ICategoryRepository : IBaseRepository<Category>;
