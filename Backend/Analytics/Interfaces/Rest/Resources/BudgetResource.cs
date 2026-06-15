namespace Buildline.Platform.Analytics.Interfaces.Rest.Resources;

/// <summary>REST resource returned by budget endpoints.</summary>
public record BudgetResource(int Id, string Project, decimal TotalBudget, decimal Spent, decimal Allocated, string Status);
