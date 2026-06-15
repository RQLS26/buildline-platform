namespace Buildline.Platform.Requisition.Domain.Model.Errors;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum RequisitionError
{
    /// <summary>Indicates that the requested requisition does not exist.</summary>
    RequisitionNotFound,
    /// <summary>Indicates that the payload does not contain the minimum data required to create a requisition.</summary>
    InvalidRequisitionData,
    /// <summary>Indicates that the project reference cannot be resolved from Analytics.</summary>
    ProjectReferenceNotFound,
    /// <summary>Indicates that the request was cancelled before the operation completed.</summary>
    OperationCancelled,
    /// <summary>Indicates that persistence failed while saving requisition data.</summary>
    DatabaseError,
    /// <summary>Indicates that an unexpected error occurred while handling a requisition command.</summary>
    InternalServerError
}

