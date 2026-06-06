using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Application.Vocabulary.Modifications.Glosses;

public sealed class GlossAddition
{
    public required GlossId Id { get; init; } 
    
    public required string Text { get; set; }
    
    public bool Primary { get; set; }
    
    public required int InsertionIndex { get; set; }
}