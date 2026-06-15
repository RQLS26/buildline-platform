using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Queries;
using Buildline.Platform.Profiles.Domain.Repositories;

namespace Buildline.Platform.Profiles.Application.Internal.QueryServices;

/// <summary>
///     Application query service for profile read use cases.
/// </summary>
public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    /// <summary>
    ///     Retrieves one company profile by identifier.
    /// </summary>
    /// <param name="query">Profile lookup query containing the requested id.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>The profile aggregate when found; otherwise <c>null</c>.</returns>
    public async Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.FindByIdAsync(query.ProfileId, cancellationToken);
    }

    /// <summary>
    ///     Retrieves every company profile registered in the platform.
    /// </summary>
    /// <param name="query">Profile listing query.</param>
    /// <param name="cancellationToken">Token used to cancel the repository call.</param>
    /// <returns>A collection of profile aggregates, possibly empty.</returns>
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.ListAsync(cancellationToken);
    }
}
