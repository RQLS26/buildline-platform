using Buildline.Platform.Shared.Domain.Model.Entities;
using Buildline.Platform.Suppliers.Interfaces.Rest.Resources;

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
    ///     Creates a supplier aggregate from the frontend supplier resource contract.
    /// </summary>
    /// <param name="resource">Supplier payload submitted from the supplier directory screen.</param>
    public Supplier(SupplierWriteResource resource)
    {
        Ruc = resource.Ruc?.Trim() ?? string.Empty;
        CompanyName = resource.CompanyName?.Trim() ?? string.Empty;
        ContactName = resource.ContactName?.Trim() ?? string.Empty;
        Email = resource.Email?.Trim() ?? string.Empty;
        Phone = resource.Phone?.Trim() ?? string.Empty;
        Rating = resource.Rating ?? 3;
        IsActive = resource.IsActive ?? true;
        Category = resource.Category?.Trim() ?? "General";
        DeliveryRate = resource.DeliveryRate ?? 80;
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
    /// <param name="resource">Resource containing only the fields that must change.</param>
    public void Apply(SupplierWriteResource resource)
    {
        Ruc = string.IsNullOrWhiteSpace(resource.Ruc) ? Ruc : resource.Ruc.Trim();
        CompanyName = string.IsNullOrWhiteSpace(resource.CompanyName) ? CompanyName : resource.CompanyName.Trim();
        ContactName = resource.ContactName is null ? ContactName : resource.ContactName.Trim();
        Email = resource.Email is null ? Email : resource.Email.Trim();
        Phone = resource.Phone is null ? Phone : resource.Phone.Trim();
        Rating = resource.Rating ?? Rating;
        IsActive = resource.IsActive ?? IsActive;
        Category = resource.Category is null ? Category : resource.Category.Trim();
        DeliveryRate = resource.DeliveryRate ?? DeliveryRate;
    }
}
