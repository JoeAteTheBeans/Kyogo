namespace Kyogo.Domain.Vocabulary.Glosses;

public sealed class Gloss
{
    public required GlossId Id { get; init; }
    
    public required string Text { get; init; }
    
    public bool Primary { get; init; }
}