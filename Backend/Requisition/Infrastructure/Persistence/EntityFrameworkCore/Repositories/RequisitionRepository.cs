using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Requisition.Domain.Repositories;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Buildline.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core repository for material requisition aggregates.
/// </summary>
/// <param name="context">Shared application database context.</param>
public class RequisitionRepository(AppDbContext context)
    : BaseRepository<Domain.Model.Aggregates.Requisition>(context), IRequisitionRepository;
