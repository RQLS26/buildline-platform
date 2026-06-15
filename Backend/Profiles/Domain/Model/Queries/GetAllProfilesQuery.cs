namespace Buildline.Platform.Profiles.Domain.Model.Queries;

/// <summary>
///     Query that requests every company profile registered in Buildline.
/// </summary>
/// <remarks>
///     This query supports TS-03. The first Sprint 3 backend version normally returns one seeded
///     company profile, but the contract allows more profiles if tenancy expands.
/// </remarks>
public record GetAllProfilesQuery;
