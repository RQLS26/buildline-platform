using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Interfaces.Rest.Resources;

namespace Buildline.Platform.Iam.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that translates sign-in REST resources into IAM commands.
/// </summary>
public static class SignInCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a sign-in command from the request resource.
    /// </summary>
    /// <param name="resource">Resource received by the sign-in endpoint.</param>
    /// <returns>A sign-in command ready for credential verification.</returns>
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}
