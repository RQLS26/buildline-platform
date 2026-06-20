using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that maps quotation aggregates to REST resources.
/// </summary>
public static class QuotationResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a quotation aggregate into the frontend quotation resource contract.
    /// </summary>
    /// <param name="quotation">Quotation aggregate retrieved from persistence.</param>
    /// <returns>Quotation REST resource.</returns>
    public static QuotationResource ToResourceFromEntity(Quotation quotation)
    {
        return new QuotationResource(
            quotation.Id,
            quotation.CompanyId,
            quotation.QuotationId,
            quotation.Supplier,
            quotation.Material,
            quotation.Project,
            quotation.Amount,
            quotation.Status,
            quotation.Date);
    }
}
