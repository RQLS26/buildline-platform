namespace Buildline.Platform.Procurement.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by quotation endpoints.
/// </summary>
/// <param name="Id">Persistence identifier used by the frontend.</param>
/// <param name="QuotationId">Business quotation code.</param>
/// <param name="Supplier">Supplier that submitted the quotation.</param>
/// <param name="Material">Quoted material.</param>
/// <param name="Project">Project associated with the quotation.</param>
/// <param name="Amount">Quoted amount.</param>
/// <param name="Status">Quotation decision status.</param>
/// <param name="Date">Display date.</param>
public record QuotationResource(
    int Id,
    string QuotationId,
    string Supplier,
    string Material,
    string Project,
    decimal Amount,
    string Status,
    string Date);
