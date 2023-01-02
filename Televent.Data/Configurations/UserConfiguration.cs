using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Televent.Core.Users.Models;

namespace Televent.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.AdditionalInfo)
            .HasColumnType("varchar");

        builder.Property(u => u.Building)
            .HasColumnType("varchar");
    }
}