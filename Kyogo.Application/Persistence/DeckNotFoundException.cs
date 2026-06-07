using Kyogo.Domain.Decks;

namespace Kyogo.Application.Persistence;

public class DeckNotFoundException(DeckId deckId) : Exception($"Deck with the id '{deckId}' not found")
{
    public DeckId DeckId { get; } = deckId;
}