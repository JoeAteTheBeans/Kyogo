using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Kyogo.Domain.Vocabulary.Glosses;
using Kyogo.Domain.Vocabulary.Senses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class TermModificationConfiguration : IEntityTypeConfiguration<TermModification>
{
    public void Configure(EntityTypeBuilder<TermModification> builder)
    {
        builder.HasKey(x => new {x.TermId, x.OwnerId});
        
        builder.Property(x => x.TermId).HasConversion(x => x.Value, x => new TermId(x));
        
        builder.Property(x => x.OwnerId).HasConversion(x => x.Value, x => new UserId(x));

        builder.OwnsMany(x => x.SenseAdditions, additionBuilder =>
        {
            additionBuilder.HasKey(x => x.Id);
            additionBuilder.Property(x => x.Id).HasConversion(x => x.Value, x => new SenseId(x));
            
            additionBuilder.Property(x => x.PartOfSpeech).HasConversion<string>();

            additionBuilder.Property(x => x.Tags)
                .HasConversion(
                    x => x.Select(y => y.ToString()).ToArray(),
                    x => x.Select(Enum.Parse<SenseTag>).ToList()
                );
            
            additionBuilder.OwnsMany(x => x.Glosses, glossBuilder =>
            {
                glossBuilder.HasKey(x => x.Id);
                glossBuilder.Property(x => x.Id).HasConversion(x  => x.Value, x => new GlossId(x));
        
                glossBuilder.Property(x => x.Text).HasMaxLength(64);
            });
        });

        builder.OwnsMany(x => x.SenseModifications, modificationBuilder =>
        {
            modificationBuilder.HasKey(x => x.SenseId);
            modificationBuilder.Property(x => x.SenseId).HasConversion(x => x.Value, x => new SenseId(x));
            
            modificationBuilder.Property(x => x.PartOfSpeechOverride).HasConversion<string?>();

            modificationBuilder.Property(x => x.TagsOverride)
                .HasConversion(
                    x => x == null ? null : x.Select(y => y.ToString()).ToArray(),
                    x => x == null ? null : x.Select(Enum.Parse<SenseTag>).ToList()
                );
            
            modificationBuilder.OwnsMany(x => x.GlossAdditions, glossAdditionBuilder =>
            {
                glossAdditionBuilder.HasKey(x => x.Id);
                glossAdditionBuilder.Property(x => x.Id).HasConversion(x  => x.Value, x => new GlossId(x));
        
                glossAdditionBuilder.Property(x => x.Text).HasMaxLength(64);
            });
            
            modificationBuilder.OwnsMany(x => x.GlossModifications, glossModificationBuilder =>
            {
                glossModificationBuilder.HasKey(x => x.ModifyGlossId);
                glossModificationBuilder.Property(x => x.ModifyGlossId).HasConversion(x  => x.Value, x => new GlossId(x));
        
                glossModificationBuilder.Property(x => x.TextOverride).HasMaxLength(64);
            });
            
            modificationBuilder.OwnsMany(x => x.GlossRemovals, glossRemovalBuilder =>
            {
                glossRemovalBuilder.HasKey(x => x.RemoveGlossId);
                glossRemovalBuilder.Property(x => x.RemoveGlossId).HasConversion(x  => x.Value, x => new GlossId(x));
            });
        });

        builder.OwnsMany(x => x.SenseRemovals, removalBuilder =>
        {
            removalBuilder.HasKey(x => x.RemoveSenseId);
            removalBuilder.Property(x => x.RemoveSenseId).HasConversion(x => x.Value, x => new SenseId(x));
        });
    }
}