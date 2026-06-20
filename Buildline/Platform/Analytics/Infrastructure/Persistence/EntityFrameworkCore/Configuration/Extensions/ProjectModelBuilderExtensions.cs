using Buildline.Platform.Analytics.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Analytics reference data module.
/// </summary>
public static class ProjectModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for project reference records.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyProjectsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Project>().HasKey(project => project.Id);
        builder.Entity<Project>().Property(project => project.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Project>().Property(project => project.CompanyId).IsRequired();
        builder.Entity<Project>().Property(project => project.Name).IsRequired().HasMaxLength(120);
        builder.Entity<Project>().Property(project => project.Location).IsRequired().HasMaxLength(120);
        builder.Entity<Project>().Property(project => project.Budget).IsRequired().HasPrecision(12, 2);
        builder.Entity<Project>().Property(project => project.Spent).IsRequired().HasPrecision(12, 2);
        builder.Entity<Project>().Property(project => project.Date).IsRequired().HasMaxLength(20);
        builder.Entity<Project>().Property(project => project.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Project>().Property(project => project.Progress).IsRequired();
    }
}

