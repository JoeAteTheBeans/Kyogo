using Kyogo.Application.Vocabulary.Modifications.Glosses;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class SenseAddition
{
    public required SenseId Id { get; init; }
    
    public required PartOfSpeech PartOfSpeech { get; set; }

    public required ICollection<CustomGloss> Glosses { get; set; }
    
    public bool Common { get; set; }

    public IList<SenseTag> Tags { get; set; } = [];
    
    public required int InsertionIndex { get; set; }
}