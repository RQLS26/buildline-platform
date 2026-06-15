using Buildline.Platform.Projects.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Projects.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Projects bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for project reference records.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    /// <remarks>
    ///     Seed values mirror the Sprint 2 json-server mock so frontend filters and dashboard
    ///     scenarios continue to work when switching from mock services to .NET Web Services.
    /// </remarks>
    public static void ApplyProjectsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Project>().HasKey(project => project.Id);
        builder.Entity<Project>().Property(project => project.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Project>().Property(project => project.Name).IsRequired().HasMaxLength(120);
        builder.Entity<Project>().Property(project => project.Location).IsRequired().HasMaxLength(120);
        builder.Entity<Project>().Property(project => project.Budget).IsRequired().HasPrecision(12, 2);
        builder.Entity<Project>().Property(project => project.Spent).IsRequired().HasPrecision(12, 2);
        builder.Entity<Project>().Property(project => project.Date).IsRequired().HasMaxLength(20);
        builder.Entity<Project>().Property(project => project.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Project>().Property(project => project.Progress).IsRequired();

        builder.Entity<Project>().HasData(
            new
            {
                Id = 1,
                Name = "Skyline Tower",
                Location = "Lima, Peru",
                Budget = 500000m,
                Spent = 120000m,
                Date = "2026-01-15",
                Status = "In Progress",
                Progress = 35
            },
            new
            {
                Id = 2,
                Name = "Coastal Bridge",
                Location = "Arequipa, Peru",
                Budget = 250000m,
                Spent = 210000m,
                Date = "2026-03-01",
                Status = "In Progress",
                Progress = 72
            },
            new
            {
                Id = 3,
                Name = "Grand Park",
                Location = "Cusco, Peru",
                Budget = 100000m,
                Spent = 105000m,
                Date = "2026-02-10",
                Status = "At Risk",
                Progress = 90
            });
    }
}
