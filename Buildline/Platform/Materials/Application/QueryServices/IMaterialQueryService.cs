using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Queries;

namespace Buildline.Platform.Materials.Application.QueryServices;

/// <summary>
///     Defines read operations exposed by the Materials application layer.
/// </summary>
/// <remarks>
///     Query services separate read use cases from write commands and keep REST controllers from
///     depending directly on Entity Framework Core repositories.
/// </remarks>
public interface IMaterialQueryService
{
    /// <summary>
    ///     Handles the query that retrieves every material.
    /// </summary>
    /// <param name="query">Query object representing the material listing request.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The materials available to requisition and inventory workflows.</returns>
    Task<IEnumerable<Material>> Handle(GetAllMaterialsQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves one material by identifier.
    /// </summary>
    /// <param name="query">Query object containing the requested material id.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The material when it exists; otherwise <c>null</c>.</returns>
    Task<Material?> Handle(GetMaterialByIdQuery query, CancellationToken cancellationToken = default);
}
