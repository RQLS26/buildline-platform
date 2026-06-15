namespace Buildline.Platform.Suppliers.Domain.Model;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum SuppliersError
{
    /// <summary>Indicates the SupplierNotFound application failure.</summary>
    SupplierNotFound,
    /// <summary>Indicates the SupplierIncidentNotFound application failure.</summary>
    SupplierIncidentNotFound,
    /// <summary>Indicates the InvalidSupplierData application failure.</summary>
    InvalidSupplierData,
    /// <summary>Indicates the InvalidSupplierIncidentData application failure.</summary>
    InvalidSupplierIncidentData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}
