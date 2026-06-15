using Buildline.Platform.Procurement.Domain.Model.Aggregates;

namespace Buildline.Platform.Procurement.Application.QueryServices;

/// <summary>
///     Application query contract for quotation read use cases.
/// </summary>
public interface IQuotationQueryService
{
    /// <summary>Retrieves every quotation.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A quotation collection, possibly empty.</returns>
    Task<IEnumerable<Quotation>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one quotation by identifier.</summary>
    /// <param name="id">Quotation persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The quotation when found; otherwise <c>null</c>.</returns>
    Task<Quotation?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
}
