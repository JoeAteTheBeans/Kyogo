using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Application.Vocabulary.Modifications.Glosses;

public sealed class GlossModification
{
    public required GlossId ModifyGlossId { get; init; }
    
    public required string? TextOverride { get; set; }
    
    public bool? PrimaryOverride { get; set; }
}