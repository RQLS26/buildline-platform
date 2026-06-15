namespace Buildline.Platform.Delivery.Domain.Model;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum DeliveryError
{
    /// <summary>Indicates the DeliveryNotFound application failure.</summary>
    DeliveryNotFound,
    /// <summary>Indicates the InvalidDeliveryData application failure.</summary>
    InvalidDeliveryData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}
