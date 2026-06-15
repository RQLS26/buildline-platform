using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Requisition.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Requisition bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for material requisitions.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyRequisitionConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Domain.Model.Aggregates.Requisition>().HasKey(requisition => requisition.Id);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.ReqId).IsRequired().HasMaxLength(32);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Material).IsRequired().HasMaxLength(120);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Project).IsRequired().HasMaxLength(120);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Quantity).IsRequired();
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Unit).IsRequired().HasMaxLength(20);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Priority).IsRequired().HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.Status).IsRequired().HasMaxLength(40);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.RequestedOn).IsRequired().HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.DeliveryDate).HasMaxLength(30);
        builder.Entity<Domain.Model.Aggregates.Requisition>().Property(requisition => requisition.RequestedBy).HasMaxLength(120);
    }
}

