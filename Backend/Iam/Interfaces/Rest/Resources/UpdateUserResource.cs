namespace Buildline.Platform.Iam.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by the user update endpoint.
/// </summary>
/// <param name="Name">Optional replacement display name.</param>
/// <param name="Email">Optional replacement sign-in email.</param>
/// <param name="Role">Optional replacement application role.</param>
/// <param name="Department">Optional replacement department or operational area.</param>
/// <param name="Phone">Optional replacement contact phone.</param>
/// <param name="AvatarColor">Optional replacement frontend avatar color token.</param>
/// <param name="IsActive">Optional replacement access-status flag.</param>
/// <remarks>
///     Every property is nullable because the endpoint is a PATCH contract used by the frontend
///     users module. Missing fields keep their current persisted value in the assembler before an
///     <see cref="Buildline.Platform.Iam.Domain.Model.Commands.UpdateUserCommand"/> is created.
/// </remarks>
public record UpdateUserResource(
    string? Name,
    string? Email,
    string? Role,
    string? Department,
    string? Phone,
    string? AvatarColor,
    bool? IsActive);
