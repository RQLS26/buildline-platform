using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Profiles.Interfaces.Rest.Resources;

namespace Buildline.Platform.Profiles.Interfaces.Rest.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
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
