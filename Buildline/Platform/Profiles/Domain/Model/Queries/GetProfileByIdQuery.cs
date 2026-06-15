namespace Buildline.Platform.Profiles.Domain.Model.Queries;

/// <summary>
///     Query that requests one company profile by identifier.
/// </summary>
/// <param name="ProfileId">Identifier of the profile requested by the API client.</param>
/// <remarks>
///     This query supports TS-01 and keeps route parameters out of repository contracts.
/// </remarks>
public record GetProfileByIdQuery(int ProfileId);
