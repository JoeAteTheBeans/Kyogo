using Kyogo.Domain.Vocabulary.Glosses;

namespace Kyogo.Application.Vocabulary;

public class ComprehensiveGloss
{
    public required GlossId Id { get; init; }
    public required string Text { get; init; }
    public required bool Primary { get; init; }
}