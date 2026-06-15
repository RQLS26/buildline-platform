using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Delivery.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Delivery bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping and constraints for deliveries.</summary>
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
    }
}

