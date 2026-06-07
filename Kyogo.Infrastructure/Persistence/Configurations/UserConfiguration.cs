using Kyogo.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new UserId(x));
        
        builder.Property(x => x.Username).HasMaxLength(32);
        builder.HasIndex(x => x.Username).IsUnique();
        
        builder.Property(x => x.Email).HasMaxLength(254);
        builder.HasIndex(x => x.Email).IsUnique();
    }
}