namespace Buildline.Platform.Requisition.Interfaces.Rest.Resources;

/// <summary>
///     REST resource returned by material requisition endpoints.
/// </summary>
/// <param name="Id">Persistence identifier used by the frontend.</param>
/// <param name="ReqId">Business requisition code.</param>
/// <param name="Material">Requested material name.</param>
/// <param name="Project">Project requesting the material.</param>
/// <param name="Quantity">Requested quantity.</param>
/// <param name="Unit">Measurement unit.</param>
/// <param name="Priority">Operational priority.</param>
/// <param name="Status">Approval workflow status.</param>
/// <param name="RequestedOn">Display date when the requisition was created.</param>
/// <param name="DeliveryDate">Requested delivery date.</param>
/// <param name="RequestedBy">Employee who created the requisition.</param>
public record RequisitionResource(
    int Id,
    string ReqId,
    string Material,
    string Project,
    int Quantity,
    string Unit,
    string Priority,
    string Status,
    string RequestedOn,
    string DeliveryDate,
    string RequestedBy);
