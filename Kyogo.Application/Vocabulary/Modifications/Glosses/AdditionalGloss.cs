using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Application.Vocabulary.Modifications.Glosses;

public sealed class AdditionalGloss
{
    public required GlossId Id { get; init; } 
    
    public required string Text { get; set; }
    
    public bool Primary { get; set; }
}