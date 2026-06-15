using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

public static class ProfileResourceFromEntityAssembler
{
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
