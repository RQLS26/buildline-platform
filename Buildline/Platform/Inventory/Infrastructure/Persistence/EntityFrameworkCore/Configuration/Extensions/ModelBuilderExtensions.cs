using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Inventory bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping and constraints for inventory items.</summary>
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
    }
}

