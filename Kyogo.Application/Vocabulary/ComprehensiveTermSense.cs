using Kyogo.Application.Vocabulary.Modifications.Glosses;
using Kyogo.Application.Vocabulary.Modifications.Senses;
using Kyogo.Domain.Vocabulary.Glosses;
using Kyogo.Domain.Vocabulary.Senses;

namespace Kyogo.Application.Vocabulary;

public sealed class ComprehensiveTermSense
{
    public SenseId Id { get; init; }
    
    public PartOfSpeech PartOfSpeech { get; init; }

    public IReadOnlyList<ComprehensiveGloss> Glosses { get; init; }
    
    public bool Common { get; init; }
    
    public IReadOnlyCollection<SenseTag> Tags { get; init; }
    
    public ComprehensiveTermSense(Sense sense, SenseModification? modification)
    {
        Id = sense.Id;
        PartOfSpeech = modification?.PartOfSpeechOverride ??  sense.PartOfSpeech;
        List<ComprehensiveGloss> glosses = [];
        foreach (Gloss gloss in sense.Glosses)
        {
            GlossModification? glossModification = modification?.GlossModifications.FirstOrDefault(x => x.ModifyGlossId == gloss.Id);
            glosses.Add(new ComprehensiveGloss 
                { 
                    Id = gloss.Id, 
                    Text = glossModification?.TextOverride ?? gloss.Text, 
                    Primary = glossModification?.PrimaryOverride ?? gloss.Primary 
                });
        }
        if (modification != null)
        {
            foreach (GlossRemoval removal in modification.GlossRemovals)
                glosses.RemoveAll(x => x.Id == removal.RemoveGlossId);
            foreach(GlossAddition addition in modification.GlossAdditions.OrderByDescending(x => x.InsertionIndex).ToList())
                glosses.Insert(addition.InsertionIndex, new ComprehensiveGloss
                {
                    Id = addition.Id, 
                    Text = addition.Text, 
                    Primary = addition.Primary
                });
        }
        Glosses = glosses;
        Common = modification?.CommonOverride ?? sense.Common;
        Tags = modification?.TagsOverride?.ToList() ?? sense.Tags;
    }
    
    public ComprehensiveTermSense(SenseAddition senseAddition)
    {
        Id = senseAddition.Id;
        PartOfSpeech = senseAddition.PartOfSpeech;
        Glosses = senseAddition.Glosses.Select(x => new ComprehensiveGloss { Id = x.Id, Text = x.Text, Primary = x.Primary }).ToList();
        Common = senseAddition.Common;
        Tags = senseAddition.Tags.ToList();
    }
}