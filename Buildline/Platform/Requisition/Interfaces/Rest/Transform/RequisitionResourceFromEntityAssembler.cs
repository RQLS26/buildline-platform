using Buildline.Platform.Requisition.Interfaces.Rest.Resources;

namespace Buildline.Platform.Requisition.Interfaces.Rest.Transform;

/// <summary>
///     Assembler that maps requisition aggregates to REST resources.
/// </summary>
public static class RequisitionResourceFromEntityAssembler
{
    /// <summary>
    ///     Converts a requisition aggregate into the frontend requisition resource contract.
    /// </summary>
    /// <param name="requisition">Requisition aggregate retrieved from persistence.</param>
    /// <returns>Requisition REST resource.</returns>
    public static RequisitionResource ToResourceFromEntity(Domain.Model.Aggregates.Requisition requisition)
    {
        return new RequisitionResource(
            requisition.Id,
            requisition.CompanyId,
            requisition.ReqId,
            requisition.Material,
            requisition.Project,
            requisition.Quantity,
            requisition.Unit,
            requisition.Priority,
            requisition.Status,
            requisition.RequestedOn,
            requisition.DeliveryDate,
            requisition.RequestedBy);
    }
}
