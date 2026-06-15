using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Suppliers bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for suppliers and supplier incidents.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplySuppliersConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Supplier>().HasKey(supplier => supplier.Id);
        builder.Entity<Supplier>().Property(supplier => supplier.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Supplier>().Property(supplier => supplier.Ruc).IsRequired().HasMaxLength(11);
        builder.Entity<Supplier>().Property(supplier => supplier.CompanyName).IsRequired().HasMaxLength(160);
        builder.Entity<Supplier>().Property(supplier => supplier.ContactName).HasMaxLength(120);
        builder.Entity<Supplier>().Property(supplier => supplier.Email).HasMaxLength(160);
        builder.Entity<Supplier>().Property(supplier => supplier.Phone).HasMaxLength(40);
        builder.Entity<Supplier>().Property(supplier => supplier.Rating).IsRequired();
        builder.Entity<Supplier>().Property(supplier => supplier.IsActive).IsRequired();
        builder.Entity<Supplier>().Property(supplier => supplier.Category).IsRequired().HasMaxLength(80);
        builder.Entity<Supplier>().Property(supplier => supplier.DeliveryRate).IsRequired();

        builder.Entity<SupplierIncident>().HasKey(incident => incident.Id);
        builder.Entity<SupplierIncident>().Property(incident => incident.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<SupplierIncident>().Property(incident => incident.IncidentId).IsRequired().HasMaxLength(24);
        builder.Entity<SupplierIncident>().Property(incident => incident.Title).IsRequired().HasMaxLength(160);
        builder.Entity<SupplierIncident>().Property(incident => incident.Description).HasMaxLength(800);
        builder.Entity<SupplierIncident>().Property(incident => incident.Supplier).HasMaxLength(160);
        builder.Entity<SupplierIncident>().Property(incident => incident.PurchaseOrder).HasMaxLength(40);
        builder.Entity<SupplierIncident>().Property(incident => incident.ReportedBy).HasMaxLength(120);
        builder.Entity<SupplierIncident>().Property(incident => incident.Severity).IsRequired().HasMaxLength(30);
        builder.Entity<SupplierIncident>().Property(incident => incident.Status).IsRequired().HasMaxLength(40);
        builder.Entity<SupplierIncident>().Property(incident => incident.Date).IsRequired().HasMaxLength(30);
        builder.Entity<SupplierIncident>().Property(incident => incident.Time).IsRequired().HasMaxLength(20);
    }
}

