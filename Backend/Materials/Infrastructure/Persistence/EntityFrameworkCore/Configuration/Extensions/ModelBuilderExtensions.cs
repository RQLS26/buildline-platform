using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Materials.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Materials bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for material catalog records.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    /// <remarks>
    ///     Seed values mirror the Sprint 2 frontend mock so inventory and requisition screens can be
    ///     validated against the .NET Web Services without changing their initial data assumptions.
    /// </remarks>
    public static void ApplyMaterialsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Material>().HasKey(material => material.Id);
        builder.Entity<Material>().Property(material => material.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Material>().Property(material => material.Sku).IsRequired().HasMaxLength(24);
        builder.Entity<Material>().Property(material => material.Name).IsRequired().HasMaxLength(120);
        builder.Entity<Material>().Property(material => material.Category).IsRequired().HasMaxLength(60);
        builder.Entity<Material>().Property(material => material.Unit).IsRequired().HasMaxLength(20);
        builder.Entity<Material>().Property(material => material.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Material>().Property(material => material.CurrentStock).IsRequired();
        builder.Entity<Material>().Property(material => material.MinStock).IsRequired();
        builder.Entity<Material>().Property(material => material.MaxStock).IsRequired();

        builder.Entity<Material>().HasData(
            new { Id = 1, Sku = "MAT-001", Name = "Steel Rebar 1/2\"", Category = "Steel", Unit = "PCS", Project = "Skyline Tower", CurrentStock = 99, MinStock = 100, MaxStock = 800 },
            new { Id = 2, Sku = "MAT-002", Name = "Concrete 3000 PSI", Category = "Concrete", Unit = "Bags", Project = "Skyline Tower", CurrentStock = 0, MinStock = 100, MaxStock = 500 },
            new { Id = 3, Sku = "MAT-003", Name = "Cement Type I", Category = "Concrete", Unit = "Bags", Project = "Coastal Bridge", CurrentStock = 200, MinStock = 50, MaxStock = 400 },
            new { Id = 4, Sku = "MAT-004", Name = "Sand Fine", Category = "Aggregate", Unit = "m3", Project = "Grand Park", CurrentStock = 0, MinStock = 50, MaxStock = 300 },
            new { Id = 5, Sku = "MAT-005", Name = "Gravel 3/4\"", Category = "Aggregate", Unit = "Tons", Project = "Skyline Tower", CurrentStock = 320, MinStock = 80, MaxStock = 500 },
            new { Id = 6, Sku = "MAT-006", Name = "PVC Pipes 4\"", Category = "Plumbing", Unit = "PCS", Project = "Coastal Bridge", CurrentStock = 45, MinStock = 60, MaxStock = 200 });
    }
}
