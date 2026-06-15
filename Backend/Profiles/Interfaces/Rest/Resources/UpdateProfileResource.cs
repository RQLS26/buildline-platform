namespace Buildline.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     Resource accepted by profile update endpoints.
/// </summary>
/// <param name="CompanyName">Replacement company name.</param>
/// <param name="Ruc">Replacement company RUC.</param>
/// <param name="Address">Replacement company address.</param>
/// <param name="Phone">Replacement company contact phone.</param>
/// <param name="Email">Replacement company contact email.</param>
public record UpdateProfileResource(string CompanyName, string Ruc, string Address, string Phone, string Email);
