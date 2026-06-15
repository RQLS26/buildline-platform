using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Procurement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for purchase order aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
public class PurchaseOrderRepository(AppDbContext context)
    : BaseRepository<PurchaseOrder>(context), IPurchaseOrderRepository;
