namespace Kyogo.Domain.Vocabulary.Components;

public sealed class KanaComponent : IComponent
{
    public string Characters => Kana;
    
    public required string Kana { get; init; }
}