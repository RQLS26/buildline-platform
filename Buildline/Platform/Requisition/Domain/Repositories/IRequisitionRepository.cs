using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Requisition.Domain.Repositories;

/// <summary>
///     Repository contract for material requisition aggregate persistence.
/// </summary>
public interface IRequisitionRepository : IBaseRepository<Domain.Model.Aggregates.Requisition>;
