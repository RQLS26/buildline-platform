namespace Buildline.Platform.Inventory.Domain.Model.Errors;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum InventoryError
{
    /// <summary>Indicates the InventoryItemNotFound application failure.</summary>
    InventoryItemNotFound,
    /// <summary>Indicates the InvalidInventoryItemData application failure.</summary>
    InvalidInventoryItemData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}

