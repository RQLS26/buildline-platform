using Buildline.Platform.Requisition.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Requisition reference data module.
/// </summary>
public static class MaterialModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for material reference records.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyMaterialsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Material>().HasKey(material => material.Id);
        builder.Entity<Material>().Property(material => material.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Material>().Property(material => material.CompanyId).IsRequired();
        builder.Entity<Material>().Property(material => material.Sku).IsRequired().HasMaxLength(24);
        builder.Entity<Material>().Property(material => material.Name).IsRequired().HasMaxLength(120);
        builder.Entity<Material>().Property(material => material.Category).IsRequired().HasMaxLength(60);
        builder.Entity<Material>().Property(material => material.Unit).IsRequired().HasMaxLength(20);
        builder.Entity<Material>().Property(material => material.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Material>().Property(material => material.CurrentStock).IsRequired();
        builder.Entity<Material>().Property(material => material.MinStock).IsRequired();
        builder.Entity<Material>().Property(material => material.MaxStock).IsRequired();
    }
}


