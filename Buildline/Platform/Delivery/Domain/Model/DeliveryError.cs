namespace Buildline.Platform.Delivery.Domain.Model;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum DeliveryError
{
    /// <summary>Indicates that the requested delivery does not exist.</summary>
    DeliveryNotFound,
    /// <summary>Indicates that the delivery payload is incomplete or invalid.</summary>
    InvalidDeliveryData,
    /// <summary>Indicates that the referenced purchase order cannot be resolved from Procurement.</summary>
    PurchaseOrderReferenceNotFound,
    /// <summary>Indicates that the request was cancelled before the operation completed.</summary>
    OperationCancelled,
    /// <summary>Indicates that persistence failed while saving delivery data.</summary>
    DatabaseError,
    /// <summary>Indicates that an unexpected error occurred while handling a delivery command.</summary>
    InternalServerError
}
