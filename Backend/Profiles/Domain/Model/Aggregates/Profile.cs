using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
///     Company profile aggregate root for a Buildline tenant.
/// </summary>
/// <remarks>
///     The current frontend consumes a company profile with company name, RUC,
///     address, phone and email. The aggregate keeps those concepts explicit
///     instead of leaking presentation DTO names into the domain.
/// </remarks>
public partial class Profile : IAuditableEntity
{
    protected Profile()
    {
        CompanyName = string.Empty;
        Ruc = string.Empty;
        Address = string.Empty;
        Phone = string.Empty;
        Email = string.Empty;
    }

    public Profile(string companyName, string ruc, string address, string phone, string email)
    {
        CompanyName = companyName;
        Ruc = ruc;
        Address = address;
        Phone = phone;
        Email = email;
    }

    public int Id { get; private set; }
    public string CompanyName { get; private set; }
    public string Ruc { get; private set; }
    public string Address { get; private set; }
    public string Phone { get; private set; }
    public string Email { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
