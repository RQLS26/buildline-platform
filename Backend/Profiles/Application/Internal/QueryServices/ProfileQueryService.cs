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
    public async Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.FindByIdAsync(query.ProfileId, cancellationToken);
    }

    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken = default)
    {
        return await profileRepository.ListAsync(cancellationToken);
    }
}
