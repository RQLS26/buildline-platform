namespace Buildline.Platform.Requisition.Domain.Model;

/// <summary>
///     Domain error codes emitted by the Requisition reference data module.
/// </summary>
/// <remarks>
///     REST assemblers translate these values to localized Problem Details responses while command
///     services remain independent from HTTP concerns.
/// </remarks>
public enum MaterialsError
{
    /// <summary>
    ///     The requested material identifier does not exist.
    /// </summary>
    MaterialNotFound,

    /// <summary>
    ///     Required reference or stock data is missing or invalid.
    /// </summary>
    InvalidMaterialData,

    /// <summary>
    ///     The material operation was cancelled before completion.
    /// </summary>
    OperationCancelled,

    /// <summary>
    ///     Persistence failed while creating, updating or deleting material data.
    /// </summary>
    DatabaseError,

    /// <summary>
    ///     An unexpected failure occurred in the Requisition reference data module.
    /// </summary>
    InternalServerError
}


