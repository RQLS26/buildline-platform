using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Buildline.Platform.Categories.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Categories.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for material category aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
/// <remarks>
///     The repository inherits generic read behavior from <see cref="BaseRepository{TEntity}"/>
///     because category administration commands are outside the current Sprint 3 scope.
/// </remarks>
public class CategoryRepository(AppDbContext context) : BaseRepository<Category>(context), ICategoryRepository;
