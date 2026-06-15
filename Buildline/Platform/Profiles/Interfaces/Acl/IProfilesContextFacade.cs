namespace Buildline.Platform.Profiles.Interfaces.Acl;

/// <summary>
///     Anti-corruption facade exposed by the Profiles bounded context.
/// </summary>
/// <remarks>
///     Other bounded contexts use this facade when they need company profile metadata without
///     depending on the Profiles aggregate or persistence model.
/// </remarks>
public interface IProfilesContextFacade
{
    /// <summary>
    ///     Fetches a profile identifier by company email.
    /// </summary>
    /// <param name="email">Company email used as the lookup value.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The profile id when the email exists; otherwise <c>0</c>.</returns>
    Task<int> FetchProfileIdByEmail(string email, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Fetches a company name by profile identifier.
    /// </summary>
    /// <param name="profileId">Identifier of the profile requested by another bounded context.</param>
    /// <param name="cancellationToken">Token used to cancel the query.</param>
    /// <returns>The company name when the profile exists; otherwise an empty string.</returns>
    Task<string> FetchCompanyNameByProfileId(int profileId, CancellationToken cancellationToken = default);
}
