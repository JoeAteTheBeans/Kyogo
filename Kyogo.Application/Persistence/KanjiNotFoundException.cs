using Kyogo.Domain.Characters;

namespace Kyogo.Application.Persistence;

public class KanjiNotFoundException(KanjiId kanjiId) : Exception($"Kanji with character {kanjiId} not found")
{
    public KanjiId KanjiId { get; } = kanjiId;
}