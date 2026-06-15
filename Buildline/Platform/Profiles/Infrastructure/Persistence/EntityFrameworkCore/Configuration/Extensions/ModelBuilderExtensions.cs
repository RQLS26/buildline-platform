using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Entity Framework Core model configuration for the Profiles bounded context.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    ///     Applies table mapping and constraints for company profiles.
    /// </summary>
    /// <param name="builder">Model builder used by the shared application database context.</param>
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Profile>().HasKey(profile => profile.Id);
        builder.Entity<Profile>().Property(profile => profile.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(profile => profile.CompanyName).IsRequired().HasMaxLength(120);
        builder.Entity<Profile>().Property(profile => profile.Ruc).IsRequired().HasMaxLength(11);
        builder.Entity<Profile>().Property(profile => profile.Address).IsRequired().HasMaxLength(180);
        builder.Entity<Profile>().Property(profile => profile.Phone).IsRequired().HasMaxLength(24);
        builder.Entity<Profile>().Property(profile => profile.Email).IsRequired().HasMaxLength(120);
    }
}

