using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class DeckSubscriptionConfiguration : IEntityTypeConfiguration<DeckSubscription>
{
    public void Configure(EntityTypeBuilder<DeckSubscription> builder)
    {
        builder.HasKey(x => new {x.DeckId, x.UserId});
        
        builder.Property(x => x.DeckId).HasConversion(x => x.Value, x => new DeckId(x));
        
        builder.Property(x => x.UserId).HasConversion(x => x.Value, x => new UserId(x));
    }
}