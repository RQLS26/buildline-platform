using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Buildline.Platform.Projects.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for construction project aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
/// <remarks>
///     The repository inherits generic list and find behavior from <see cref="BaseRepository{TEntity}"/>
///     because the first Projects API contract is read-only.
/// </remarks>
public class ProjectRepository(AppDbContext context) : BaseRepository<Project>(context), IProjectRepository;
