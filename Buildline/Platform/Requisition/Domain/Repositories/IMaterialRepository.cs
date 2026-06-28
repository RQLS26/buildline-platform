using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Requisition.Domain.Repositories;

/// <summary>
///     Repository contract for material aggregate persistence.
/// </summary>
/// <remarks>
///     The current Materials context uses generic repository operations for list, lookup, create,
///     update and delete. The explicit interface preserves the bounded-context dependency boundary.
/// </remarks>
public interface IMaterialRepository : IBaseRepository<Material>, ICompanyScopedRepository<Material>;
