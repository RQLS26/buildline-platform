namespace Buildline.Platform.Analytics.Interfaces.Rest.Resources;

/// <summary>Resource accepted by budget create and partial update endpoints.</summary>
public record BudgetWriteResource(string? Project, decimal? TotalBudget, decimal? Spent, decimal? Allocated, string? Status);
