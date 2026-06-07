using Kyogo.Domain.Characters;
using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class DeckConfiguration : IEntityTypeConfiguration<Deck>
{
    public void Configure(EntityTypeBuilder<Deck> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new DeckId(x));

        builder.Property(x => x.OwnerId)
            .HasConversion(
                x => x.HasValue ? x.Value.Value : (Guid?)null, 
                x => x.HasValue ? new UserId(x.Value) : null
            );
        
        builder.Property(x => x.Name).HasMaxLength(128);
        
        builder.Property(x => x.Description).HasMaxLength(5000);
        
        builder.Property(x => x.Terms)
            .HasConversion(
                x => x.Select(t => t.Value).ToArray(),
                x => x.Select(t => new TermId(t)).ToList()
            );
        
        builder.Property(x => x.Kanji)
            .HasConversion(
                x => x.Select(k => k.Value).ToArray(),
                x => x.Select(k => new KanjiId(k)).ToList()
            );
        
        builder.HasDiscriminator<string>("deck_type")
            .HasValue<Deck>("deck")
            .HasValue<SubDeck>("sub_deck");
    }
}