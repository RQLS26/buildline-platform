using Buildline.Platform.Procurement.Application.QueryServices;
using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Repositories;

namespace Buildline.Platform.Procurement.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates quotation reads.
/// </summary>
/// <param name="quotationRepository">Repository used to retrieve quotations.</param>
public class QuotationQueryService(IQuotationRepository quotationRepository) : IQuotationQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Quotation>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await quotationRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Quotation?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await quotationRepository.FindByIdAsync(id, cancellationToken);
    }
}
