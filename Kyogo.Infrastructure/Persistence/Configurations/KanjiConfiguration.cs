using Kyogo.Domain.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class KanjiConfiguration : IEntityTypeConfiguration<Kanji>
{
    public void Configure(EntityTypeBuilder<Kanji> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new KanjiId(x));
        
        builder.Property(x => x.OnyoumiReadings)
            .HasConversion(
                x => x.ToArray(),
                x => x.ToList()
            );
        
        builder.Property(x => x.KunyoumiReadings)
            .HasConversion(
                x => x.ToArray(),
                x => x.ToList()
            );
        
        builder.Property(x => x.Meanings)
            .HasConversion(
                x => x.ToArray(),
                x => x.ToList()
            );
    }
}