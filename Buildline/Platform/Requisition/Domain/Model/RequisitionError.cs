namespace Buildline.Platform.Requisition.Domain.Model;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum RequisitionError
{
    /// <summary>Indicates the RequisitionNotFound application failure.</summary>
    RequisitionNotFound,
    /// <summary>Indicates the InvalidRequisitionData application failure.</summary>
    InvalidRequisitionData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}
