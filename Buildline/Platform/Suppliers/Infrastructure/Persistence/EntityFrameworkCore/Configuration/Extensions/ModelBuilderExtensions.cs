using Buildline.Platform.Suppliers.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Suppliers.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Suppliers bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for suppliers and supplier incidents.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    /// <remarks>
    ///     Seed values mirror the Sprint 2 json-server resources so supplier directory and incident
    ///     screens can switch to the .NET Web Services without losing their demonstration data.
    /// </remarks>
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

        builder.Entity<Supplier>().HasData(
            new { Id = 1, Ruc = "20100055237", CompanyName = "ABC Supplies Inc.", ContactName = "Roberto Sanchez", Email = "ventas@abcsupplies.com", Phone = "+51 987 111 222", Rating = 5, IsActive = true, Category = "Steel", DeliveryRate = 95 },
            new { Id = 2, Ruc = "20419381272", CompanyName = "BuildMore Materials", ContactName = "Maria Lopez", Email = "contacto@buildmore.com", Phone = "+51 987 333 444", Rating = 4, IsActive = true, Category = "Concrete", DeliveryRate = 88 },
            new { Id = 3, Ruc = "20555555555", CompanyName = "Steel House Ltd.", ContactName = "Pedro Rojas", Email = "info@steelhouse.com", Phone = "+51 987 555 666", Rating = 4, IsActive = true, Category = "Steel", DeliveryRate = 78 },
            new { Id = 4, Ruc = "10777777777", CompanyName = "Global Construction", ContactName = "Lucia Vargas", Email = "global@construction.com", Phone = "+51 987 777 888", Rating = 3, IsActive = true, Category = "General", DeliveryRate = 82 },
            new { Id = 5, Ruc = "20888888888", CompanyName = "Cement Plus", ContactName = "Jorge Diaz", Email = "ventas@cementplus.com", Phone = "+51 987 999 000", Rating = 3, IsActive = false, Category = "Concrete", DeliveryRate = 70 });

        builder.Entity<SupplierIncident>().HasData(
            new { Id = 1, IncidentId = "INC-015", Title = "Delayed delivery of steel rebar", Description = "Steel rebar shipment from Steel House Ltd. is 3 days overdue.", Supplier = "Steel House Ltd.", PurchaseOrder = "PO-2026-0012", ReportedBy = "Carlos Mendoza", Severity = "High", Status = "Open", Date = "May 15, 2026", Time = "10:45 AM" },
            new { Id = 2, IncidentId = "INC-014", Title = "Wrong cement type delivered", Description = "Received Type II cement instead of Type I as specified in the PO.", Supplier = "BuildMore Materials", PurchaseOrder = "PO-2026-0013", ReportedBy = "Ana Garcia", Severity = "High", Status = "In Progress", Date = "May 15, 2026", Time = "08:30 AM" },
            new { Id = 3, IncidentId = "INC-013", Title = "Damaged packaging on arrival", Description = "Several bags arrived with torn packaging.", Supplier = "ABC Supplies Inc.", PurchaseOrder = "PO-2026-0010", ReportedBy = "James Wilson", Severity = "Medium", Status = "Resolved", Date = "May 16, 2026", Time = "04:15 PM" });
    }
}
