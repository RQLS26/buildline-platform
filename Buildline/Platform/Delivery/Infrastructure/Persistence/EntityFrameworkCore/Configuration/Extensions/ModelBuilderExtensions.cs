using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Delivery bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping, constraints and seed data for deliveries.</summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyDeliveryConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Domain.Model.Aggregates.Delivery>().HasKey(delivery => delivery.Id);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.TrackingId).IsRequired().HasMaxLength(32);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.PurchaseOrder).IsRequired().HasMaxLength(40);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Supplier).IsRequired().HasMaxLength(160);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Origin).HasMaxLength(160);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Destination).HasMaxLength(160);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Eta).HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.DispatchDate).HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Delivery>().Property(delivery => delivery.Items).HasMaxLength(240);

        builder.Entity<Domain.Model.Aggregates.Delivery>().HasData(
            new { Id = 1, TrackingId = "TRK-0048", PurchaseOrder = "PO-2026-0015", Supplier = "ABC Supplies Inc.", Origin = "Lima Warehouse", Destination = "Skyline Tower Site", Status = "In Transit", Eta = "May 21, 2026", DispatchDate = "May 19, 2026", Items = "Steel Rebar 1/2\" (500 PCS)" },
            new { Id = 2, TrackingId = "TRK-0047", PurchaseOrder = "PO-2026-0013", Supplier = "BuildMore Materials", Origin = "Arequipa Plant", Destination = "Coastal Bridge Site", Status = "Delivered", Eta = "May 18, 2026", DispatchDate = "May 16, 2026", Items = "Concrete 3000 PSI (200 Bags)" },
            new { Id = 3, TrackingId = "TRK-0046", PurchaseOrder = "PO-2026-0012", Supplier = "Steel House Ltd.", Origin = "Callao Port", Destination = "Grand Park Site", Status = "Delayed", Eta = "May 17, 2026", DispatchDate = "May 14, 2026", Items = "Steel Beams W8 (120 PCS)" });
    }
}
