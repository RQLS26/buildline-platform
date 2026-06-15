using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Procurement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Procurement bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and seed data for purchase orders and quotations.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyProcurementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<PurchaseOrder>().HasKey(order => order.Id);
        builder.Entity<PurchaseOrder>().Property(order => order.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PurchaseOrder>().Property(order => order.OrderId).IsRequired().HasMaxLength(32);
        builder.Entity<PurchaseOrder>().Property(order => order.Date).IsRequired().HasMaxLength(30);
        builder.Entity<PurchaseOrder>().Property(order => order.SupplierName).IsRequired().HasMaxLength(160);
        builder.Entity<PurchaseOrder>().Property(order => order.Material).IsRequired().HasMaxLength(160);
        builder.Entity<PurchaseOrder>().Property(order => order.Project).IsRequired().HasMaxLength(120);
        builder.Entity<PurchaseOrder>().Property(order => order.TotalAmount).IsRequired().HasPrecision(12, 2);
        builder.Entity<PurchaseOrder>().Property(order => order.Status).IsRequired().HasMaxLength(40);

        builder.Entity<Quotation>().HasKey(quotation => quotation.Id);
        builder.Entity<Quotation>().Property(quotation => quotation.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Quotation>().Property(quotation => quotation.QuotationId).IsRequired().HasMaxLength(32);
        builder.Entity<Quotation>().Property(quotation => quotation.Supplier).IsRequired().HasMaxLength(160);
        builder.Entity<Quotation>().Property(quotation => quotation.Material).IsRequired().HasMaxLength(160);
        builder.Entity<Quotation>().Property(quotation => quotation.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Quotation>().Property(quotation => quotation.Amount).IsRequired().HasPrecision(12, 2);
        builder.Entity<Quotation>().Property(quotation => quotation.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Quotation>().Property(quotation => quotation.Date).IsRequired().HasMaxLength(30);

        builder.Entity<PurchaseOrder>().HasData(
            new { Id = 1, OrderId = "PO-2026-0015", Date = "May 19, 2026", SupplierName = "ABC Supplies Inc.", Material = "Steel Rebar 1/2\"", Project = "Skyline Tower", TotalAmount = 125000m, Status = "Approved" },
            new { Id = 2, OrderId = "PO-2026-0014", Date = "May 18, 2026", SupplierName = "BuildMore Materials", Material = "Cement Type I", Project = "Skyline Tower", TotalAmount = 85200m, Status = "Pending" },
            new { Id = 3, OrderId = "PO-2026-0013", Date = "May 18, 2026", SupplierName = "BuildMore Materials", Material = "Concrete 3000 PSI", Project = "Coastal Bridge", TotalAmount = 42500m, Status = "Approved" },
            new { Id = 4, OrderId = "PO-2026-0012", Date = "May 17, 2026", SupplierName = "Steel House Ltd.", Material = "Steel Beams W8", Project = "Grand Park", TotalAmount = 67800m, Status = "Pending" });

        builder.Entity<Quotation>().HasData(
            new { Id = 1, QuotationId = "QT-2026-0018", Supplier = "ABC Supplies Inc.", Material = "Steel Rebar 1/2\"", Project = "Skyline Tower", Amount = 12500m, Status = "Pending", Date = "May 19, 2026" },
            new { Id = 2, QuotationId = "QT-2026-0017", Supplier = "Cement Plus", Material = "Concrete 3000 PSI", Project = "Skyline Tower", Amount = 8300m, Status = "Accepted", Date = "May 18, 2026" },
            new { Id = 3, QuotationId = "QT-2026-0016", Supplier = "Global Construction", Material = "Power Drill Set", Project = "Coastal Bridge", Amount = 3450m, Status = "Accepted", Date = "May 17, 2026" },
            new { Id = 4, QuotationId = "QT-2026-0015", Supplier = "BuildMore Materials", Material = "Cement Type I", Project = "Skyline Tower", Amount = 15800m, Status = "Pending", Date = "May 17, 2026" });
    }
}
