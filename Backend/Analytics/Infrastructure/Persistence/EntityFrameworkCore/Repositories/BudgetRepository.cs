using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Buildline.Platform.Analytics.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Entity Framework Core repository for budget aggregates.</summary>
/// <param name="context">Shared application database context.</param>
public class BudgetRepository(AppDbContext context) : BaseRepository<Budget>(context), IBudgetRepository;
