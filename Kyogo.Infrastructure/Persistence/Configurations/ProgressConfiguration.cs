using Kyogo.Application.SpacedRepetition;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kyogo.Infrastructure.Persistence.Configurations;

public sealed class ProgressConfiguration : IEntityTypeConfiguration<Progress>
{
    public void Configure(EntityTypeBuilder<Progress> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ProgressId(x));

        builder.Property(x => x.TermId).HasConversion(x => x.Value, x => new TermId(x));

        builder.Property(x => x.OwnerId).HasConversion(x => x.Value, x => new UserId(x));

        builder.Property(x => x.Stage).HasConversion<string>();
    }
}