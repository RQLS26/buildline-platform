namespace Buildline.Platform.Profiles.Domain.Model.Commands;

/// <summary>
///     Command that requests replacement of a company profile.
/// </summary>
/// <param name="ProfileId">Identifier of the profile aggregate to update.</param>
/// <param name="CompanyName">Replacement company name.</param>
/// <param name="Ruc">Replacement company RUC.</param>
/// <param name="Address">Replacement company address.</param>
/// <param name="Phone">Replacement company contact phone.</param>
/// <param name="Email">Replacement company contact email.</param>
/// <remarks>
///     This command supports TS-02 and is also used by the PATCH-compatible profile endpoint.
/// </remarks>
public record UpdateProfileCommand(
    int ProfileId,
    string CompanyName,
    string Ruc,
    string Address,
    string Phone,
    string Email);
