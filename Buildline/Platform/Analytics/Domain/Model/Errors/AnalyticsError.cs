namespace Buildline.Platform.Analytics.Domain.Model.Errors;

/// <summary>
///     Enumerates application-level failures emitted by command services in this bounded context.
/// </summary>
public enum AnalyticsError
{
    /// <summary>Indicates the BudgetNotFound application failure.</summary>
    BudgetNotFound,
    /// <summary>Indicates the InvalidBudgetData application failure.</summary>
    InvalidBudgetData,
    /// <summary>Indicates the OperationCancelled application failure.</summary>
    OperationCancelled,
    /// <summary>Indicates the DatabaseError application failure.</summary>
    DatabaseError,
    /// <summary>Indicates the InternalServerError application failure.</summary>
    InternalServerError
}

