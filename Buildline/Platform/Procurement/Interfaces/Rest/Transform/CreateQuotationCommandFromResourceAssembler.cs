using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateQuotationCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateQuotationCommand ToCommandFromResource(QuotationWriteResource resource, int companyId = 1)
    {
        return new CreateQuotationCommand(
            resource.QuotationId,
            resource.Supplier,
            resource.Material,
            resource.Project,
            resource.Amount,
            resource.Status,
            resource.Date,
            companyId);
    }
}
