namespace Buildline.Platform.Procurement.Domain.Model.Errors;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum ProcurementError
{
    /// <summary>Indicates that the requested purchase order does not exist.</summary>
    PurchaseOrderNotFound,
    /// <summary>Indicates that the requested quotation does not exist.</summary>
    QuotationNotFound,
    /// <summary>Indicates that the purchase order payload is incomplete or invalid.</summary>
    InvalidPurchaseOrderData,
    /// <summary>Indicates that the quotation payload is incomplete or invalid.</summary>
    InvalidQuotationData,
    /// <summary>Indicates that the supplier reference is missing, unknown or inactive.</summary>
    SupplierReferenceNotSelectable,
    /// <summary>Indicates that the request was cancelled before the operation completed.</summary>
    OperationCancelled,
    /// <summary>Indicates that persistence failed while saving procurement data.</summary>
    DatabaseError,
    /// <summary>Indicates that an unexpected error occurred while handling a procurement command.</summary>
    InternalServerError
}

