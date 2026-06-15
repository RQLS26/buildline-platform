using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Procurement.Domain.Repositories;

/// <summary>
///     Repository contract for purchase order aggregate persistence.
/// </summary>
public interface IPurchaseOrderRepository : IBaseRepository<PurchaseOrder>;
