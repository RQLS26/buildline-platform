using Buildline.Platform.Profiles.Application.QueryServices;
using Buildline.Platform.Profiles.Domain.Model.Queries;
using Buildline.Platform.Profiles.Interfaces.Acl;

namespace Buildline.Platform.Profiles.Application.Acl;

/// <summary>
///     Application implementation of the Profiles anti-corruption facade.
/// </summary>
/// <param name="profileQueryService">Profiles query service used to retrieve company profile metadata.</param>
/// <remarks>
///     The facade exposes profile metadata through primitive values so external contexts can reference
///     company profile information without importing the Profiles domain model.
/// </remarks>
public class ProfilesContextFacade(IProfileQueryService profileQueryService) : IProfilesContextFacade
{
    /// <inheritdoc />
    public async Task<int> FetchProfileIdByEmail(string email, CancellationToken cancellationToken = default)
    {
        var profiles = await profileQueryService.Handle(new GetAllProfilesQuery(), cancellationToken);
        var profile = profiles.FirstOrDefault(candidate =>
            string.Equals(candidate.Email, email, StringComparison.OrdinalIgnoreCase));
        return profile?.Id ?? 0;
    }

    /// <inheritdoc />
    public async Task<string> FetchCompanyNameByProfileId(int profileId, CancellationToken cancellationToken = default)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(profileId), cancellationToken);
        return profile?.CompanyName ?? string.Empty;
    }
}
