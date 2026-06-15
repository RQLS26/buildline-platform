using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Analytics and Budgeting bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping, constraints and seed data for project budgets.</summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Budget>().HasKey(budget => budget.Id);
        builder.Entity<Budget>().Property(budget => budget.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Budget>().Property(budget => budget.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Budget>().Property(budget => budget.TotalBudget).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Spent).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Allocated).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Status).IsRequired().HasMaxLength(40);

        builder.Entity<Budget>().HasData(
            new { Id = 1, Project = "Skyline Tower", TotalBudget = 500000m, Spent = 120000m, Allocated = 350000m, Status = "On Track" },
            new { Id = 2, Project = "Coastal Bridge", TotalBudget = 250000m, Spent = 210000m, Allocated = 240000m, Status = "At Risk" },
            new { Id = 3, Project = "Grand Park", TotalBudget = 100000m, Spent = 105000m, Allocated = 100000m, Status = "Over Budget" });
    }
}
