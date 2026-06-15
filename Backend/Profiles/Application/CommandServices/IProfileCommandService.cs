using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Profiles.Application.CommandServices;

/// <summary>
///     Defines write operations exposed by the Profiles application layer.
/// </summary>
/// <remarks>
///     Profile commands coordinate company profile updates while keeping REST controllers independent
///     from persistence details.
/// </remarks>
public interface IProfileCommandService
{
    /// <summary>
    ///     Handles company profile update.
    /// </summary>
    /// <param name="command">Command containing the profile id and replacement company fields.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence operations.</param>
    /// <returns>A result containing the updated profile or a profile-domain error.</returns>
    Task<Result<Profile>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken = default);
}
