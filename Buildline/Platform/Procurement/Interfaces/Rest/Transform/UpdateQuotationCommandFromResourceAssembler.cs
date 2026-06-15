using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateQuotationCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="quotationId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateQuotationCommand ToCommandFromResource(int quotationId, QuotationWriteResource resource)
    {
        return new UpdateQuotationCommand(
            quotationId,
            resource.QuotationId,
            resource.Supplier,
            resource.Material,
            resource.Project,
            resource.Amount,
            resource.Status,
            resource.Date);
    }
}
