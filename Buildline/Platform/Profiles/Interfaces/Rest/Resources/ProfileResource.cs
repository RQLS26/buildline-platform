namespace Buildline.Platform.Profiles.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by profile endpoints.
/// </summary>
/// <param name="Id">Profile identifier used by API clients.</param>
/// <param name="CompanyName">Company name displayed in profile settings.</param>
/// <param name="Ruc">Company RUC.</param>
/// <param name="Address">Company address.</param>
/// <param name="Phone">Company contact phone.</param>
/// <param name="Email">Company contact email.</param>
public record ProfileResource(int Id, string CompanyName, string Ruc, string Address, string Phone, string Email);
