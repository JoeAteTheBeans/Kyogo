using Kyogo.Domain.Decks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class SubDeckConfiguration : IEntityTypeConfiguration<SubDeck>
{
    public void Configure(EntityTypeBuilder<SubDeck> builder)
    {
        builder.Property(x => x.ParentId)
            .HasConversion(x => x.Value, x => new DeckId(x));
    }
}