using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Analytics and Budgeting bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping and constraints for project budgets.</summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyAnalyticsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Budget>().HasKey(budget => budget.Id);
        builder.Entity<Budget>().Property(budget => budget.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Budget>().Property(budget => budget.CompanyId).IsRequired();
        builder.Entity<Budget>().Property(budget => budget.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Budget>().Property(budget => budget.TotalBudget).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Spent).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Allocated).IsRequired().HasPrecision(12, 2);
        builder.Entity<Budget>().Property(budget => budget.Status).IsRequired().HasMaxLength(40);
    }
}

