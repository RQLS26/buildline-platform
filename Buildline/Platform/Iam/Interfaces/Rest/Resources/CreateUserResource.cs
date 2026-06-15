namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by the create-user administration endpoint.
/// </summary>
/// <param name="Name">Display name for the managed user.</param>
/// <param name="Email">Unique email used as the sign-in identifier.</param>
/// <param name="Password">Initial plain text password that will be hashed by IAM.</param>
/// <param name="Role">Application role assigned by an administrator.</param>
/// <param name="Department">Operational department associated with the account.</param>
/// <param name="Phone">Contact phone shown in the frontend users table.</param>
/// <param name="AvatarColor">Frontend avatar fallback color token.</param>
/// <param name="IsActive">Flag indicating whether the account can authenticate.</param>
/// <remarks>
///     The initial last-login value is assigned by the assembler because it is backend-owned session
///     metadata, not an editable frontend field during user creation.
/// </remarks>
public record CreateUserResource(
    string Name,
    string Email,
    string Password,
    string Role,
    string Department,
    string Phone,
    string AvatarColor,
    bool IsActive);
