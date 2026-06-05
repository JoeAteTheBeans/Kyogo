namespace Kyogo.Domain.Decks;

public sealed class SubDeck : Deck
{
    public required DeckId ParentId { get; init; }
}