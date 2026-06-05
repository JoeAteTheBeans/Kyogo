using Kyogo.Domain.Characters;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Domain.Decks;

public class Deck
{
    public required DeckId Id { get; init; }
    
    public UserId? OwnerId { get; init; }
    
    public required string Name { get; init; }
    
    public string? Description { get; init; }
    
    public string? ArtworkPath { get; init; }
    
    public required IList<TermId> Terms { get; init; }
    
    public required IList<KanjiId> Kanji { get; init; }
}