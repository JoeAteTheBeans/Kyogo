namespace Kyogo.Application.Vocabulary.Modifications.Glosses;

public sealed class GlossAddition
{
    public required int InsertionIndex { get; set; }
    
    public required AdditionalGloss AdditionalGloss { get; init; }
}