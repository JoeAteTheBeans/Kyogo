namespace Kyogo.Domain.Vocabulary.Components;

public sealed class JukujikunComponent : IComponent
{
    public string Characters => KanjiCharacters;
    
    public required string KanjiCharacters { get; init; }
    
    public required string KanaReading  { get; init; }
}