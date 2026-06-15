namespace Buildline.Platform.Requisition.Domain.Model.Commands;

/// <summary>
///     Command that requests creation of a material requisition aggregate from a REST write payload.
/// </summary>
/// <param name="ReqId">Write value for the ReqId field in the frontend-compatible contract.</param>
/// <param name="Material">Write value for the Material field in the frontend-compatible contract.</param>
/// <param name="Project">Write value for the Project field in the frontend-compatible contract.</param>
/// <param name="Quantity">Write value for the Quantity field in the frontend-compatible contract.</param>
/// <param name="Unit">Write value for the Unit field in the frontend-compatible contract.</param>
/// <param name="Priority">Write value for the Priority field in the frontend-compatible contract.</param>
/// <param name="Status">Write value for the Status field in the frontend-compatible contract.</param>
/// <param name="RequestedOn">Write value for the RequestedOn field in the frontend-compatible contract.</param>
/// <param name="DeliveryDate">Write value for the DeliveryDate field in the frontend-compatible contract.</param>
/// <param name="RequestedBy">Write value for the RequestedBy field in the frontend-compatible contract.</param>
/// <remarks>
///     The command keeps HTTP resources outside the domain model and lets the application service own
///     validation, persistence coordination and error translation.
/// </remarks>
public record CreateRequisitionCommand(
    string? ReqId,
    string? Material,
    string? Project,
    int? Quantity,
    string? Unit,
    string? Priority,
    string? Status,
    string? RequestedOn,
    string? DeliveryDate,
    string? RequestedBy);
