using Buildline.Platform.Categories.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Categories.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Categories bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for material categories.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    /// <remarks>
    ///     Seed values mirror the material categories used by the Sprint 2 frontend and the Materials
    ///     API seed data, ensuring `/api/v1/categories` can replace the json-server mock cleanly.
    /// </remarks>
    public static void ApplyCategoriesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Category>().HasKey(category => category.Id);
        builder.Entity<Category>().Property(category => category.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Category>().Property(category => category.Name).IsRequired().HasMaxLength(60);
        builder.Entity<Category>().Property(category => category.Description).IsRequired().HasMaxLength(180);

        builder.Entity<Category>().HasData(
            new { Id = 1, Name = "Steel", Description = "Structural steel materials such as rebar and beams." },
            new { Id = 2, Name = "Concrete", Description = "Cement and concrete materials for construction work." },
            new { Id = 3, Name = "Aggregate", Description = "Sand, gravel and related aggregate materials." },
            new { Id = 4, Name = "Plumbing", Description = "Pipes and plumbing-related materials." },
            new { Id = 5, Name = "Electrical", Description = "Electrical wiring and installation materials." },
            new { Id = 6, Name = "Wood", Description = "Plywood and wood-based construction materials." },
            new { Id = 7, Name = "Equipment", Description = "Safety and site operation equipment." },
            new { Id = 8, Name = "General", Description = "General construction supplies." });
    }
}
