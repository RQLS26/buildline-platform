using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Profiles.Domain.Repositories;

/// <summary>
///     Repository contract for company profile aggregate persistence.
/// </summary>
/// <remarks>
///     The current profile context uses generic repository operations, but the explicit interface
///     preserves the bounded-context boundary for future profile-specific queries.
/// </remarks>
public interface IProfileRepository : IBaseRepository<Profile>;
