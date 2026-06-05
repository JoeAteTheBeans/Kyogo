using Kyogo.Application.Vocabulary.Modifications.Glosses;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class TermSenseModification
{
    public required SenseId SenseId { get; init; }
    
    public PartOfSpeech? PartOfSpeechOverride { get; set; }
    
    public ICollection<GlossRemoval> GlossRemovals { get; } = [];

    public ICollection<GlossModification> GlossModifications { get; } = [];

    public ICollection<GlossAddition> GlossAdditions { get; } = [];
    
    public bool? CommonOverride { get; set; }

    public IList<SenseTag>? TagsOverride { get; set; }
}