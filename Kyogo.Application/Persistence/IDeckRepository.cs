using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;

namespace Kyogo.Application.Persistence;

public interface IDeckRepository
{
    public Task<Deck> GetAsync(DeckId deckId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Deck>> GetManyAsync(IEnumerable<DeckId> deckIds, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Deck>> GetSubDecksAsync(DeckId deckId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Deck>> GetSubDecksOfManyAsync(IEnumerable<DeckId> deckIds, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Deck>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Deck>> GetAllUnownedAsync(CancellationToken cancellationToken = default);
    public Task AddAsync(Deck deck, CancellationToken cancellationToken = default);
    public Task RemoveAsync(Deck deck, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Deck deck, CancellationToken cancellationToken = default);
}