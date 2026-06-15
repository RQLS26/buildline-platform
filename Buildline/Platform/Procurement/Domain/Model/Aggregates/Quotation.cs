using Buildline.Platform.Procurement.Interfaces.Rest.Resources;
using Buildline.Platform.Shared.Domain.Model.Entities;

namespace Buildline.Platform.Procurement.Domain.Model.Aggregates;

/// <summary>
///     Aggregate root that represents a supplier quotation received during procurement comparison.
/// </summary>
public partial class Quotation : IAuditableEntity
{
    /// <summary>Initializes an empty quotation for Entity Framework Core materialization.</summary>
    protected Quotation()
    {
        QuotationId = string.Empty;
        Supplier = string.Empty;
        Material = string.Empty;
        Project = string.Empty;
        Status = string.Empty;
        Date = string.Empty;
    }

    /// <summary>
    ///     Creates a quotation from the procurement quotation contract.
    /// </summary>
    /// <param name="resource">Quotation payload entered by logistics staff.</param>
    public Quotation(QuotationWriteResource resource)
    {
        QuotationId = string.IsNullOrWhiteSpace(resource.QuotationId) ? $"QT-{DateTime.UtcNow:yyyyMMddHHmmss}" : resource.QuotationId.Trim();
        Supplier = resource.Supplier?.Trim() ?? string.Empty;
        Material = resource.Material?.Trim() ?? string.Empty;
        Project = resource.Project?.Trim() ?? string.Empty;
        Amount = resource.Amount ?? 0m;
        Status = resource.Status?.Trim() ?? "Pending";
        Date = resource.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated quotation identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the business quotation code displayed in comparison tables.</summary>
    public string QuotationId { get; private set; }

    /// <summary>Gets the supplier that submitted the quotation.</summary>
    public string Supplier { get; private set; }

    /// <summary>Gets the quoted material.</summary>
    public string Material { get; private set; }

    /// <summary>Gets the project associated with the quotation.</summary>
    public string Project { get; private set; }

    /// <summary>Gets the quoted amount.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Gets the quotation decision status.</summary>
    public string Status { get; private set; }

    /// <summary>Gets the display date when the quotation was received.</summary>
    public string Date { get; private set; }

    /// <summary>Gets or sets the audit timestamp captured when the quotation is created.</summary>
    public DateTimeOffset? CreatedAt { get; set; }

    /// <summary>Gets or sets the audit timestamp captured when the quotation is updated.</summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    ///     Applies a partial quotation update, including accepted/rejected transitions.
    /// </summary>
    /// <param name="resource">Quotation fields to replace.</param>
    public void Apply(QuotationWriteResource resource)
    {
        QuotationId = resource.QuotationId is null ? QuotationId : resource.QuotationId.Trim();
        Supplier = resource.Supplier is null ? Supplier : resource.Supplier.Trim();
        Material = resource.Material is null ? Material : resource.Material.Trim();
        Project = resource.Project is null ? Project : resource.Project.Trim();
        Amount = resource.Amount ?? Amount;
        Status = resource.Status is null ? Status : resource.Status.Trim();
        Date = resource.Date is null ? Date : resource.Date.Trim();
    }
}
