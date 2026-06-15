using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Buildline.Platform.Inventory.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Entity Framework Core repository for inventory item aggregates.</summary>
/// <param name="context">Shared application database context.</param>
public class InventoryItemRepository(AppDbContext context)
    : BaseRepository<InventoryItem>(context), IInventoryItemRepository;
