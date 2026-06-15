using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Queries;

namespace Buildline.Platform.Profiles.Application.QueryServices;

public interface IProfileQueryService
{
    Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken = default);
}
