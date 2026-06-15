using Buildline.Platform.Requisition.Domain.Model.Commands;
using Buildline.Platform.Requisition.Interfaces.Rest.Resources;

namespace Buildline.Platform.Requisition.Interfaces.Rest.Transform;

/// <summary>
///     Converts REST write resources into update commands for the application layer.
/// </summary>
public static class UpdateRequisitionCommandFromResourceAssembler
{
    /// <summary>
    ///     Builds an update command from the route identifier and HTTP request body.
    /// </summary>
    /// <param name="requisitionId">Aggregate identifier from the route.</param>
    /// <param name="resource">Resource received by the REST endpoint.</param>
    /// <returns>A command containing the target aggregate id and nullable replacement values.</returns>
    public static UpdateRequisitionCommand ToCommandFromResource(int requisitionId, RequisitionWriteResource resource)
    {
        return new UpdateRequisitionCommand(
            requisitionId,
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
