using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the IAM bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints and indexes for IAM.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(user => user.Id);
        builder.Entity<User>().Property(user => user.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(user => user.Name).IsRequired().HasMaxLength(120);
        builder.Entity<User>().Property(user => user.Email).IsRequired().HasMaxLength(120);
        builder.Entity<User>().HasIndex(user => user.Email).IsUnique();
        builder.Entity<User>().Property(user => user.PasswordHash).IsRequired().HasMaxLength(120);
        builder.Entity<User>().Property(user => user.Role).IsRequired().HasMaxLength(30);
        builder.Entity<User>().Property(user => user.Department).IsRequired().HasMaxLength(80);
        builder.Entity<User>().Property(user => user.Phone).HasMaxLength(24);
        builder.Entity<User>().Property(user => user.AvatarColor).IsRequired().HasMaxLength(12);
        builder.Entity<User>().Property(user => user.IsActive).IsRequired();
        builder.Entity<User>().Property(user => user.LastLogin).IsRequired().HasMaxLength(40);
    }
}

