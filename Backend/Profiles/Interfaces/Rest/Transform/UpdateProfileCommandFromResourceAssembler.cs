using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates profile update REST resources into application commands.
/// </summary>
public static class UpdateProfileCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update-profile command from the route identifier and request resource.
    /// </summary>
    /// <param name="profileId">Identifier extracted from the endpoint route.</param>
    /// <param name="resource">Resource received by the update-profile endpoint.</param>
    /// <returns>A command ready for application validation and aggregate mutation.</returns>
    public static UpdateProfileCommand ToCommandFromResource(int profileId, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand(
            profileId,
            resource.CompanyName,
            resource.Ruc,
            resource.Address,
            resource.Phone,
            resource.Email);
    }
}
