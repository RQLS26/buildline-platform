using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Materials.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for material aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
/// <remarks>
///     The repository inherits generic CRUD behavior from <see cref="BaseRepository{TEntity}"/> while
///     preserving a Materials-specific dependency boundary for application services.
/// </remarks>
public class MaterialRepository(AppDbContext context) : BaseRepository<Material>(context), IMaterialRepository;
