using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Domain.Vocabulary.Senses;

public sealed class Sense
{
    public required SenseId Id { get; init; }
    
    public required PartOfSpeech PartOfSpeech { get; init; }

    public required IReadOnlyList<Gloss> Glosses { get; init; }
    
    public bool Common { get; init; }

    public IReadOnlyCollection<SenseTag> Tags { get; init; } = [];
}