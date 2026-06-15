using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that converts profile aggregates into REST resources.
/// </summary>
/// <remarks>
///     The assembler keeps controllers from exposing domain aggregates directly and stabilizes the
///     API contract consumed by the frontend profile form.
/// </remarks>
public static class ProfileResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a company profile aggregate to the resource returned by profile endpoints.
    /// </summary>
    /// <param name="profile">Profile aggregate retrieved from persistence.</param>
    /// <returns>A frontend-compatible profile resource.</returns>
    public static ProfileResource ToResourceFromEntity(Profile profile)
    {
        return new ProfileResource(
            profile.Id,
            profile.CompanyName,
            profile.Ruc,
            profile.Address,
            profile.Phone,
            profile.Email);
    }
}
