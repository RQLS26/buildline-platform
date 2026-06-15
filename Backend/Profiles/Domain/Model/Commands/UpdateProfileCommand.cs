namespace Buildline.Platform.Profiles.Domain.Model.Commands;

public record UpdateProfileCommand(
    int ProfileId,
    string CompanyName,
    string Ruc,
    string Address,
    string Phone,
    string Email);
