using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Queries;

namespace Buildline.Platform.Profiles.Application.QueryServices;

/// <summary>
///     Defines read operations exposed by the Profiles application layer.
/// </summary>
public interface IProfileQueryService
{
    /// <summary>
    ///     Handles the query that retrieves one profile by identifier.
    /// </summary>
    /// <param name="query">Query object containing the requested profile id.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The profile when it exists; otherwise <c>null</c>.</returns>
    Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles the query that retrieves every company profile.
    /// </summary>
    /// <param name="query">Query object representing the profile listing request.</param>
    /// <param name="cancellationToken">Token used to cancel repository access.</param>
    /// <returns>The registered company profiles.</returns>
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken = default);
}
