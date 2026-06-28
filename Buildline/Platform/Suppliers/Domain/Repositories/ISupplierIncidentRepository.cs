using Buildline.Platform.Shared.Domain.Repositories;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;

namespace Buildline.Platform.Suppliers.Domain.Repositories;

/// <summary>
///     Repository contract for supplier incident aggregate persistence.
/// </summary>
public interface ISupplierIncidentRepository : IBaseRepository<SupplierIncident>, ICompanyScopedRepository<SupplierIncident>;
