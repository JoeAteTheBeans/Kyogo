namespace Kyogo.Domain.Vocabulary.Components;

public sealed class KanjiComponent : IComponent
{
    public string Characters => Kanji;
    
    public required string Kanji { get; init; }
    
    public required string KanaReading  { get; init; }
}