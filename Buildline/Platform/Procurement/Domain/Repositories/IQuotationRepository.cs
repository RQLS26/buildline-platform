using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Procurement.Domain.Repositories;

/// <summary>
///     Repository contract for quotation aggregate persistence.
/// </summary>
public interface IQuotationRepository : IBaseRepository<Quotation>, ICompanyScopedRepository<Quotation>;
