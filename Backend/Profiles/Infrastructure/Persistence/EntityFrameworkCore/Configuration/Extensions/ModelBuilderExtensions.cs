using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Buildline.Platform.Profiles.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfilesConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Profile>().HasKey(profile => profile.Id);
        builder.Entity<Profile>().Property(profile => profile.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(profile => profile.CompanyName).IsRequired().HasMaxLength(120);
        builder.Entity<Profile>().Property(profile => profile.Ruc).IsRequired().HasMaxLength(11);
        builder.Entity<Profile>().Property(profile => profile.Address).IsRequired().HasMaxLength(180);
        builder.Entity<Profile>().Property(profile => profile.Phone).IsRequired().HasMaxLength(24);
        builder.Entity<Profile>().Property(profile => profile.Email).IsRequired().HasMaxLength(120);

        builder.Entity<Profile>().HasData(
            new
            {
                Id = 1,
                CompanyName = "Buildline S.A.C.",
                Ruc = "20555444333",
                Address = "Av. Primavera 123, Surco, Lima",
                Phone = "+51 987 654 321",
                Email = "contacto@buildline.com"
            });
    }
}
