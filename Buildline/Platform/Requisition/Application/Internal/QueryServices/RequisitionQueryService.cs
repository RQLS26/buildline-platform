using Buildline.Platform.Requisition.Application.QueryServices;
using Buildline.Platform.Requisition.Domain.Repositories;

namespace Buildline.Platform.Requisition.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates material requisition reads.
/// </summary>
/// <param name="requisitionRepository">Repository used to retrieve requisition aggregates.</param>
public class RequisitionQueryService(IRequisitionRepository requisitionRepository) : IRequisitionQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Model.Aggregates.Requisition>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await requisitionRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Domain.Model.Aggregates.Requisition?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await requisitionRepository.FindByIdAsync(id, cancellationToken);
    }
    /// <inheritdoc />
    public async Task<IEnumerable<Domain.Model.Aggregates.Requisition>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await requisitionRepository.ListByCompanyIdAsync(companyId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Domain.Model.Aggregates.Requisition?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default)
    {
        return await requisitionRepository.FindByIdAndCompanyIdAsync(id, companyId, cancellationToken);
    }
}
