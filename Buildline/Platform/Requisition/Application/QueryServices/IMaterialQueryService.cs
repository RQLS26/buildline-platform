using Buildline.Platform.Requisition.Domain.Model.Aggregates;

namespace Buildline.Platform.Requisition.Application.QueryServices;

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
    ///     Lists every material currently registered.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The materials available to requisition and inventory workflows.</returns>
    Task<IEnumerable<Material>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Finds one material by its unique identifier.
    /// </summary>
    /// <param name="materialId">The identifier of the material to look up.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The material when it exists; otherwise <c>null</c>.</returns>
    Task<Material?> FindByIdAsync(int materialId, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Material>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Material?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
