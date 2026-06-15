namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by the sign-in endpoint.
/// </summary>
/// <param name="Email">Email submitted as the sign-in identifier.</param>
/// <param name="Password">Plain text password submitted for verification.</param>
public record SignInResource(string Email, string Password);
