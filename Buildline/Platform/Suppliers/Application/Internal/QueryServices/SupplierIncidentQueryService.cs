using Buildline.Platform.Suppliers.Application.QueryServices;
using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Buildline.Platform.Suppliers.Domain.Repositories;

namespace Buildline.Platform.Suppliers.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates supplier incident board reads.
/// </summary>
/// <param name="incidentRepository">Repository used to retrieve incident aggregates.</param>
public class SupplierIncidentQueryService(ISupplierIncidentRepository incidentRepository) : ISupplierIncidentQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<SupplierIncident>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await incidentRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<SupplierIncident?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await incidentRepository.FindByIdAsync(id, cancellationToken);
    }
}
