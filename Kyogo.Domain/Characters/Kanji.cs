namespace Kyogo.Domain.Characters;

public sealed class Kanji
{
    public required KanjiId Id { get; init; }

    public string Character => Id.ToString();

    public IReadOnlyList<string> OnyoumiReadings { get; init; } = [];

    public IReadOnlyList<string> KunyoumiReadings { get; init; } = [];
    
    public required IReadOnlyList<string> Meanings { get; init; }
}