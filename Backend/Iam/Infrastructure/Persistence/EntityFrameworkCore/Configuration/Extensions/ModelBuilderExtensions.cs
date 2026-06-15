using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Iam.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the IAM bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping, constraints, indexes and seed users for IAM.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    /// <remarks>
    ///     Seed users mirror the Sprint 2 frontend mock accounts so the first backend version can be
    ///     validated with known credentials. Password hashes are fixed pre-generated BCrypt values so
    ///     migrations remain deterministic.
    /// </remarks>
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

        builder.Entity<User>().HasData(
            new
            {
                Id = 1,
                Name = "Nombre admin",
                Email = "admin@buildline.com",
                PasswordHash = "$2a$11$ZIgOUFd7cA0EDVQ7KXkxleNzW8rkPUHGbWp7PXuLyvOZxEWeVXQkm",
                Role = "owner",
                Department = "Management",
                Phone = "+51 987 654 321",
                AvatarColor = "#3d63a1",
                IsActive = true,
                LastLogin = "May 19, 2026"
            },
            new
            {
                Id = 2,
                Name = "Carlos Mendoza",
                Email = "carlos@buildline.com",
                PasswordHash = "$2a$11$jpepzAUUxa.fRn8vJgBuQ.n4SDJFFeS/iEfg.7yjprthBADPSXbBy",
                Role = "viewer",
                Department = "Engineering",
                Phone = "+51 912 345 678",
                AvatarColor = "#F96116",
                IsActive = true,
                LastLogin = "May 18, 2026"
            });
    }
}
