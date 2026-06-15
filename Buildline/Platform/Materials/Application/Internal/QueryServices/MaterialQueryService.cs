using Buildline.Platform.Materials.Application.QueryServices;
using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Queries;
using Buildline.Platform.Materials.Domain.Repositories;

namespace Buildline.Platform.Materials.Application.Internal.QueryServices;

/// <summary>
///     Application query service that coordinates read access to material catalog data.
/// </summary>
/// <param name="materialRepository">Repository used to retrieve persisted material aggregates.</param>
/// <remarks>
///     The service keeps material listing and detail retrieval explicit as TS-04 and TS-06 use cases.
/// </remarks>
public class MaterialQueryService(IMaterialRepository materialRepository) : IMaterialQueryService
{
    /// <summary>
    ///     Retrieves every material currently registered in the catalog.
    /// </summary>
    /// <param name="query">Material listing query.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A collection of material aggregates, possibly empty.</returns>
    public async Task<IEnumerable<Material>> Handle(GetAllMaterialsQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRepository.ListAsync(cancellationToken);
    }

    /// <summary>
    ///     Retrieves one material by identifier.
    /// </summary>
    /// <param name="query">Material lookup query containing the requested id.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The material aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<Material?> Handle(GetMaterialByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await materialRepository.FindByIdAsync(query.MaterialId, cancellationToken);
    }
}
