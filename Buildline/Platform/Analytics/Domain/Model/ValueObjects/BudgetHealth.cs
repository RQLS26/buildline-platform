namespace Buildline.Platform.Analytics.Domain.Model.ValueObjects;

/// <summary>Calculated health states used by budget dashboards.</summary>
public enum BudgetHealth
{
    /// <summary>Spending remains inside expected budget limits.</summary>
    OnTrack,
    /// <summary>Spending is close to the approved budget limit.</summary>
    AtRisk,
    /// <summary>Spending exceeds the approved budget.</summary>
    OverBudget
}
