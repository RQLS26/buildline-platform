using System.Text.Json.Serialization;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Iam.Domain.Model.Aggregates;

/// <summary>
///     User aggregate root for Buildline identity and role management.
/// </summary>
public partial class User : IAuditableEntity
{
    protected User()
    {
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        Role = string.Empty;
        Department = string.Empty;
        Phone = string.Empty;
        AvatarColor = string.Empty;
        LastLogin = string.Empty;
    }

    public User(
        string name,
        string email,
        string passwordHash,
        string role,
        string department,
        string phone,
        string avatarColor,
        bool isActive,
        string lastLogin)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Department = department;
        Phone = phone;
        AvatarColor = avatarColor;
        IsActive = isActive;
        LastLogin = lastLogin;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    [JsonIgnore] public string PasswordHash { get; private set; }
    public string Role { get; private set; }
    public string Department { get; private set; }
    public string Phone { get; private set; }
    public string AvatarColor { get; private set; }
    public bool IsActive { get; private set; }
    public string LastLogin { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
