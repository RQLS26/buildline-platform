using Buildline.Platform.Inventory.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Inventory reference data module.
/// </summary>
public static class CategoryModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for material categories.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyCategoriesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Category>().HasKey(category => category.Id);
        builder.Entity<Category>().Property(category => category.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Category>().Property(category => category.Name).IsRequired().HasMaxLength(60);
        builder.Entity<Category>().Property(category => category.Description).IsRequired().HasMaxLength(180);
    }
}

