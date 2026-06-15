using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Profiles.Application.CommandServices;

public interface IProfileCommandService
{
    Task<Result<Profile>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken = default);
}
