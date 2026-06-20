using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Procurement.Interfaces.Rest.Resources;

namespace Buildline.Platform.Procurement.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreatePurchaseOrderCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="companyId">Company profile identifier resolved from the company-scoped route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreatePurchaseOrderCommand ToCommandFromResource(PurchaseOrderWriteResource resource, int companyId = 1)
    {
        return new CreatePurchaseOrderCommand(
            resource.OrderId,
            resource.Date,
            resource.SupplierName,
            resource.Material,
            resource.Project,
            resource.TotalAmount,
            resource.Status,
            companyId);
    }
}
