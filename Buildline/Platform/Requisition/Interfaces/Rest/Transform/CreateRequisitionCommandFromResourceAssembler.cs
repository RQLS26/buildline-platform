using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;

namespace Buildline.Platform.Requisition.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into create commands for the application layer.
/// </summary>
public static class CreateRequisitionCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds a create command from the HTTP request body.
    /// </summary>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the same write values without exposing REST types to the domain.</returns>
    public static CreateRequisitionCommand ToCommandFromResource(RequisitionWriteResource resource)
    {
        return new CreateRequisitionCommand(
            resource.ReqId,
            resource.Material,
            resource.Project,
            resource.Quantity,
            resource.Unit,
            resource.Priority,
            resource.Status,
            resource.RequestedOn,
            resource.DeliveryDate,
            resource.RequestedBy);
    }
}
