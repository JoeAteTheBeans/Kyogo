using Kyogo.Application.Vocabulary.Modifications.Glosses;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary.Modifications.Senses;

public sealed class AdditionalTermSense
{
    public SenseId Id { get; init; }
    
    public PartOfSpeech PartOfSpeech { get; set; }

    public ICollection<AdditionalGloss> Glosses { get; set; } = [];
    
    public bool Common { get; set; }

    public IList<SenseTag> Tags { get; set; } = [];
}