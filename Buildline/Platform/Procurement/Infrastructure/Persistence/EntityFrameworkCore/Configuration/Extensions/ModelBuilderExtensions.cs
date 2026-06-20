using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Procurement.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Procurement bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for purchase orders and quotations.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyProcurementConfiguration(this ModelBuilder builder)
    {
        builder.Entity<PurchaseOrder>().HasKey(order => order.Id);
        builder.Entity<PurchaseOrder>().Property(order => order.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PurchaseOrder>().Property(order => order.CompanyId).IsRequired();
        builder.Entity<PurchaseOrder>().Property(order => order.OrderId).IsRequired().HasMaxLength(32);
        builder.Entity<PurchaseOrder>().Property(order => order.Date).IsRequired().HasMaxLength(30);
        builder.Entity<PurchaseOrder>().Property(order => order.SupplierName).IsRequired().HasMaxLength(160);
        builder.Entity<PurchaseOrder>().Property(order => order.Material).IsRequired().HasMaxLength(160);
        builder.Entity<PurchaseOrder>().Property(order => order.Project).IsRequired().HasMaxLength(120);
        builder.Entity<PurchaseOrder>().Property(order => order.TotalAmount).IsRequired().HasPrecision(12, 2);
        builder.Entity<PurchaseOrder>().Property(order => order.Status).IsRequired().HasMaxLength(40);

        builder.Entity<Quotation>().HasKey(quotation => quotation.Id);
        builder.Entity<Quotation>().Property(quotation => quotation.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Quotation>().Property(quotation => quotation.CompanyId).IsRequired();
        builder.Entity<Quotation>().Property(quotation => quotation.QuotationId).IsRequired().HasMaxLength(32);
        builder.Entity<Quotation>().Property(quotation => quotation.Supplier).IsRequired().HasMaxLength(160);
        builder.Entity<Quotation>().Property(quotation => quotation.Material).IsRequired().HasMaxLength(160);
        builder.Entity<Quotation>().Property(quotation => quotation.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Quotation>().Property(quotation => quotation.Amount).IsRequired().HasPrecision(12, 2);
        builder.Entity<Quotation>().Property(quotation => quotation.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Quotation>().Property(quotation => quotation.Date).IsRequired().HasMaxLength(30);
    }
}

