namespace Buildline.Platform.Communication.Domain.Model.Errors;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum CommunicationError
{
    /// <summary>Indicates the MessageNotFound application failure.</summary>
    MessageNotFound,
    /// <summary>Indicates the InvalidMessageData application failure.</summary>
    InvalidMessageData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}

