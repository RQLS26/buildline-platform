using Buildline.Platform.Delivery.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>Entity Framework Core repository for delivery aggregates.</summary>
/// <param name="context">Shared application database context.</param>
public class DeliveryRepository(AppDbContext context)
    : BaseRepository<Domain.Model.Aggregates.Delivery>(context), IDeliveryRepository;
