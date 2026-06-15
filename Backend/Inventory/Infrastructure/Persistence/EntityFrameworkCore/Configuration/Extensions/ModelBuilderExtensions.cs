using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Inventory bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping, constraints and seed data for inventory items.</summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyInventoryConfiguration(this ModelBuilder builder)
    {
        builder.Entity<InventoryItem>().HasKey(item => item.Id);
        builder.Entity<InventoryItem>().Property(item => item.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<InventoryItem>().Property(item => item.Sku).IsRequired().HasMaxLength(24);
        builder.Entity<InventoryItem>().Property(item => item.Name).IsRequired().HasMaxLength(120);
        builder.Entity<InventoryItem>().Property(item => item.Project).IsRequired().HasMaxLength(120);
        builder.Entity<InventoryItem>().Property(item => item.Category).IsRequired().HasMaxLength(80);
        builder.Entity<InventoryItem>().Property(item => item.CurrentStock).IsRequired();
        builder.Entity<InventoryItem>().Property(item => item.MaxStock).IsRequired();
        builder.Entity<InventoryItem>().Property(item => item.MinStock).IsRequired();
        builder.Entity<InventoryItem>().Property(item => item.LastUpdated).IsRequired().HasMaxLength(30);

        builder.Entity<InventoryItem>().HasData(
            new { Id = 1, Sku = "INV-001", Name = "Steel Rebar 1/2\"", Project = "Skyline Tower", Category = "Steel", CurrentStock = 99, MaxStock = 800, MinStock = 100, LastUpdated = "2026-05-16" },
            new { Id = 2, Sku = "INV-002", Name = "Concrete 3000 PSI", Project = "Skyline Tower", Category = "Concrete", CurrentStock = 0, MaxStock = 500, MinStock = 100, LastUpdated = "2026-05-16" },
            new { Id = 3, Sku = "INV-003", Name = "Cement Type I", Project = "Coastal Bridge", Category = "Concrete", CurrentStock = 200, MaxStock = 400, MinStock = 50, LastUpdated = "2026-05-17" },
            new { Id = 4, Sku = "INV-004", Name = "Sand Fine", Project = "Grand Park", Category = "Aggregate", CurrentStock = 0, MaxStock = 300, MinStock = 50, LastUpdated = "2026-05-17" });
    }
}
