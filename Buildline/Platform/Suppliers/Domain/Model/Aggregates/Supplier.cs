using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Suppliers.Domain.Model.Commands;

namespace Buildline.Platform.Suppliers.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a supplier available for procurement operations.
/// </summary>
/// <remarks>
///     Suppliers are selected by purchase orders, quotations and incident records. The aggregate keeps
///     commercial identity, SUNAT-facing RUC data and operational performance indicators together so
///     Procurement can make decisions without duplicating supplier profile fields.
/// </remarks>
public partial class Supplier : IAuditableEntity
{
    /// <summary>
    ///     Initializes an empty supplier instance for Entity Framework Core materialization.
    /// </summary>
    protected Supplier()
    {
        Ruc = string.Empty;
        CompanyName = string.Empty;
        ContactName = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Category = string.Empty;
    }

    /// <summary>
    ///     Creates a supplier aggregate from the frontend supplier command contract.
    /// </summary>
    /// <param name="command">Supplier payload submitted from the supplier directory screen.</param>
    public Supplier(CreateSupplierCommand command)
    {
        Ruc = command.Ruc?.Trim() ?? string.Empty;
        CompanyName = command.CompanyName?.Trim() ?? string.Empty;
        ContactName = command.ContactName?.Trim() ?? string.Empty;
        Email = command.Email?.Trim() ?? string.Empty;
        Phone = command.Phone?.Trim() ?? string.Empty;
        Rating = command.Rating ?? 3;
        IsActive = command.IsActive ?? true;
        Category = command.Category?.Trim() ?? "General";
        DeliveryRate = command.DeliveryRate ?? 80;
    }

    /// <summary>
    ///     Gets the database-generated supplier identifier.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    ///     Gets the Peruvian tax identifier used for supplier validation workflows.
    /// </summary>
    public string Ruc { get; private set; }

    /// <summary>
    ///     Gets the legal or commercial company name displayed by the supplier directory.
    /// </summary>
    public string CompanyName { get; private set; }

    /// <summary>
    ///     Gets the primary contact person for operational coordination.
    /// </summary>
    public string ContactName { get; private set; }

    /// <summary>
    ///     Gets the email address used to send quotations and purchase orders.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    ///     Gets the phone number used by logistics staff for urgent coordination.
    /// </summary>
    public string Phone { get; private set; }

    /// <summary>
    ///     Gets the supplier rating shown in the frontend supplier ranking.
    /// </summary>
    public int Rating { get; private set; }

    /// <summary>
    ///     Gets whether the supplier can be selected for new procurement processes.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    ///     Gets the material category where the supplier performs best.
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    ///     Gets the historical on-time delivery percentage used by analytics.
    /// </summary>
    public int DeliveryRate { get; private set; }

    /// <summary>
    ///     Gets or sets the audit timestamp captured when the supplier is created.
    /// </summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the audit timestamp captured when the supplier is updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Applies a partial supplier update received from the supplier directory workflow.
    /// </summary>
    /// <param name="command">Command containing only the fields that must change.</param>
    public void Apply(UpdateSupplierCommand command)
    {
        Ruc = string.IsNullOrWhiteSpace(command.Ruc) ? Ruc : command.Ruc.Trim();
        CompanyName = string.IsNullOrWhiteSpace(command.CompanyName) ? CompanyName : command.CompanyName.Trim();
        ContactName = command.ContactName is null ? ContactName : command.ContactName.Trim();
        Email = command.Email is null ? Email : command.Email.Trim();
        Phone = command.Phone is null ? Phone : command.Phone.Trim();
        Rating = command.Rating ?? Rating;
        IsActive = command.IsActive ?? IsActive;
        Category = command.Category is null ? Category : command.Category.Trim();
        DeliveryRate = command.DeliveryRate ?? DeliveryRate;
    }
}



