using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Communication bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping, constraints and seed data for inbox messages.</summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyCommunicationConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Message>().HasKey(message => message.Id);
        builder.Entity<Message>().Property(message => message.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Message>().Property(message => message.Sender).IsRequired().HasMaxLength(120);
        builder.Entity<Message>().Property(message => message.Subject).IsRequired().HasMaxLength(180);
        builder.Entity<Message>().Property(message => message.Preview).HasMaxLength(500);
        builder.Entity<Message>().Property(message => message.Icon).HasMaxLength(40);
        builder.Entity<Message>().Property(message => message.IconClass).HasMaxLength(60);
        builder.Entity<Message>().Property(message => message.Label).HasMaxLength(60);
        builder.Entity<Message>().Property(message => message.LabelClass).HasMaxLength(60);
        builder.Entity<Message>().Property(message => message.IsRead).IsRequired();
        builder.Entity<Message>().Property(message => message.Starred).IsRequired();
        builder.Entity<Message>().Property(message => message.Category).IsRequired().HasMaxLength(40);
        builder.Entity<Message>().Property(message => message.Time).HasMaxLength(30);
        builder.Entity<Message>().Property(message => message.Date).HasMaxLength(30);

        builder.Entity<Message>().HasData(
            new { Id = 1, Sender = "System", Subject = "PO-2026-0015 Approved", Preview = "The purchase order for ABC Supplies has been approved and is ready for dispatch.", Icon = "pi-check-circle", IconClass = "icon-success", Label = "", LabelClass = "", IsRead = true, Starred = false, Category = "updates", Time = "2 min", Date = "2026-05-19" },
            new { Id = 2, Sender = "Inventory System", Subject = "Low Stock: Concrete 3000 PSI", Preview = "Inventory at Skyline Tower dropped below minimum threshold.", Icon = "pi-exclamation-triangle", IconClass = "icon-warning", Label = "Critical", LabelClass = "label-critical", IsRead = false, Starred = false, Category = "alerts", Time = "15 min", Date = "2026-05-19" },
            new { Id = 3, Sender = "Logistics", Subject = "Delivery TRK-0048 In Transit", Preview = "Shipment from ABC Supplies has departed Lima warehouse.", Icon = "pi-truck", IconClass = "icon-info", Label = "", LabelClass = "", IsRead = false, Starred = false, Category = "updates", Time = "1 hr", Date = "2026-05-19" });
    }
}
