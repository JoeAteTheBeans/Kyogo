using Kyogo.Domain.Users;

namespace Kyogo.Domain.Decks;

public sealed class DeckSubscription
{
    public required UserId UserId { get; init; }
    
    public required DeckId DeckId { get; init; }
}