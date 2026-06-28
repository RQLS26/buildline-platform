using Buildline.Platform.Communication.Domain.Model.Aggregates;

namespace Buildline.Platform.Communication.Application.QueryServices;

/// <summary>Application query contract for inbox read use cases.</summary>
public interface IMessageQueryService
{
    /// <summary>Retrieves every message.</summary>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A message collection, possibly empty.</returns>
    Task<IEnumerable<Message>> ListAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one message by identifier.</summary>
    /// <param name="id">Message persistence identifier.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The message when found; otherwise <c>null</c>.</returns>
    Task<Message?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Retrieves rows owned by the supplied company profile.
    /// </summary>
    /// <param name="companyId">Company profile identifier that owns the requested rows.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A company-owned collection, possibly empty.</returns>
    Task<IEnumerable<Message>> ListByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves one row by identifier and company ownership.
    /// </summary>
    /// <param name="id">Persistence identifier requested by the API client.</param>
    /// <param name="companyId">Company profile identifier that must own the row.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The owned row when found; otherwise <c>null</c>.</returns>
    Task<Message?> FindByIdAndCompanyIdAsync(int id, int companyId, CancellationToken cancellationToken = default);
}
