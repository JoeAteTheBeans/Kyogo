using System.Text.Json;
using Kyogo.Domain.Vocabulary;
using Kyogo.Domain.Vocabulary.Components;
using Kyogo.Domain.Vocabulary.Glosses;
using Kyogo.Domain.Vocabulary.Senses;
using Kyogo.Infrastructure.Serialisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class TermConfiguration : IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new TermId(x));
        
        var jsonOptions = new JsonSerializerOptions();
        jsonOptions.Converters.Add(new ComponentJsonConverter());
        builder.Property(x => x.Components)
            .HasConversion(
                x => JsonSerializer.Serialize(x, jsonOptions),
                x => JsonSerializer.Deserialize<IReadOnlyList<IComponent>>(x, jsonOptions)!
            );
        
        builder.OwnsMany(x => x.Senses, senseBuilder =>
        {
            senseBuilder.HasKey(x => x.Id);
            senseBuilder.Property(x => x.Id).HasConversion(x => x.Value, x => new SenseId(x));
            
            senseBuilder.Property(x => x.PartOfSpeech).HasConversion<string>();

            senseBuilder.Property(x => x.Tags)
                .HasConversion(
                    x => x.Select(y => y.ToString()).ToArray(),
                    x => x.Select(Enum.Parse<SenseTag>).ToList()
                );

            senseBuilder.OwnsMany(x => x.Glosses, glossBuilder =>
            {
                glossBuilder.HasKey(x => x.Id);
                glossBuilder.Property(x => x.Id).HasConversion(x  => x.Value, x => new GlossId(x));
        
                glossBuilder.Property(x => x.Text).HasMaxLength(64);
            });
        });
    }
}