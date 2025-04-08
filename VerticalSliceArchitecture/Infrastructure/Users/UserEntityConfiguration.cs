using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerticalSliceArchitecture.Domain.Users;

namespace VerticalSliceArchitecture.Infrastructure.Users;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new UserId(value));

        builder.Property(u => u.Name).IsRequired();
        builder.Property(u => u.Email).IsRequired();
    }
}