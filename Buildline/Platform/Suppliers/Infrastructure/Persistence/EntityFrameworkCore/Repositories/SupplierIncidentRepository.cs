using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Repositories;

namespace Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for supplier incident aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
public class SupplierIncidentRepository(AppDbContext context)
    : BaseRepository<SupplierIncident>(context), ISupplierIncidentRepository;
