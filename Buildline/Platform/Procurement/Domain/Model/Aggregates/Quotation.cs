using Buildline.Platform.Procurement.Domain.Model.Commands;
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
    /// <param name="command">Quotation payload entered by logistics staff.</param>
    public Quotation(CreateQuotationCommand command)
    {
        CompanyId = command.CompanyId;
        QuotationId = string.IsNullOrWhiteSpace(command.QuotationId) ? $"QT-{DateTime.UtcNow:yyyyMMddHHmmss}" : command.QuotationId.Trim();
        Supplier = command.Supplier?.Trim() ?? string.Empty;
        Material = command.Material?.Trim() ?? string.Empty;
        Project = command.Project?.Trim() ?? string.Empty;
        Amount = command.Amount ?? 0m;
        Status = command.Status?.Trim() ?? "Pending";
        Date = command.Date?.Trim() ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
    }

    /// <summary>Gets the database-generated quotation identifier.</summary>
    public int Id { get; private set; }

    /// <summary>Gets the company profile identifier that owns this operational record.</summary>
    public int CompanyId { get; private set; }

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
    /// <param name="command">Quotation fields to replace.</param>
    public void Apply(UpdateQuotationCommand command)
    {
        QuotationId = command.QuotationId is null ? QuotationId : command.QuotationId.Trim();
        Supplier = command.Supplier is null ? Supplier : command.Supplier.Trim();
        Material = command.Material is null ? Material : command.Material.Trim();
        Project = command.Project is null ? Project : command.Project.Trim();
        Amount = command.Amount ?? Amount;
        Status = command.Status is null ? Status : command.Status.Trim();
        Date = command.Date is null ? Date : command.Date.Trim();
    }
}



