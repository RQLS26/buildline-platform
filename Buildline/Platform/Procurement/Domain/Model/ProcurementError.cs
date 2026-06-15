namespace Buildline.Platform.Procurement.Domain.Model;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum ProcurementError
{
    /// <summary>Indicates the PurchaseOrderNotFound application failure.</summary>
    PurchaseOrderNotFound,
    /// <summary>Indicates the QuotationNotFound application failure.</summary>
    QuotationNotFound,
    /// <summary>Indicates the InvalidPurchaseOrderData application failure.</summary>
    InvalidPurchaseOrderData,
    /// <summary>Indicates the InvalidQuotationData application failure.</summary>
    InvalidQuotationData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}
