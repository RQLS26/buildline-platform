namespace Buildline.Platform.Inventory.Domain.Model;

/// <summary>
///     Domain error codes emitted by the Inventory reference data module.
/// </summary>
/// <remarks>
///     The REST layer translates these errors into localized Problem Details responses, keeping
///     category query logic independent from HTTP status-code decisions.
/// </remarks>
public enum CategoriesError
{
    /// <summary>
    ///     The requested material category identifier does not exist.
    /// </summary>
    CategoryNotFound,

    /// <summary>
    ///     Category state or request data violates reference-data rules.
    /// </summary>
    InvalidCategoryData,

    /// <summary>
    ///     The category operation was cancelled before completion.
    /// </summary>
    OperationCancelled,

    /// <summary>
    ///     Persistence failed while accessing category data.
    /// </summary>
    DatabaseError,

    /// <summary>
    ///     An unexpected failure occurred in the Inventory reference data module.
    /// </summary>
    InternalServerError
}



