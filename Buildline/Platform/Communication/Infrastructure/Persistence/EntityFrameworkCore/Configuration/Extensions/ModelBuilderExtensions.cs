using Buildline.Platform.Communication.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Communication.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>Entity Framework Core model configuration for the Communication bounded context.</summary>
public static class ModelBuilderExtensions
{
    /// <summary>Applies table mapping and constraints for inbox messages.</summary>
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
    }
}

