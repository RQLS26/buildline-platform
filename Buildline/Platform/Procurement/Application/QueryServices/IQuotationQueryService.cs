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
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Quotation>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Quotation?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
