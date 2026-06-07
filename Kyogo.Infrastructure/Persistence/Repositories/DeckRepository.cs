using Kyogo.Application.Persistence;
using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class DeckRepository(KyogoDbContext context) : IDeckRepository
{
    public async Task<Deck> GetAsync(DeckId deckId, CancellationToken cancellationToken = default)
        => await context.Decks.FirstOrDefaultAsync(x => x.Id == deckId, cancellationToken)
           ??  throw new DeckNotFoundException(deckId);

    public async Task<IReadOnlyCollection<Deck>> GetManyAsync(IEnumerable<DeckId> deckIds, CancellationToken cancellationToken = default)
        => await context.Decks.Where(x => deckIds.Contains(x.Id)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Deck>> GetSubDecksAsync(DeckId deckId, CancellationToken cancellationToken = default)
        => await context.Decks.OfType<SubDeck>().Where(x => x.ParentId == deckId).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Deck>> GetSubDecksOfManyAsync(IEnumerable<DeckId> deckIds, CancellationToken cancellationToken = default)
        => await context.Decks.OfType<SubDeck>().Where(x => deckIds.Contains(x.ParentId)).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Deck>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default)
        => await context.Decks.Where(x => x.OwnerId == userId).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Deck>> GetAllUnownedAsync(CancellationToken cancellationToken = default)
        => await context.Decks.Where(x => x.OwnerId == null).ToListAsync(cancellationToken);

    public async Task AddAsync(Deck deck, CancellationToken cancellationToken = default)
        => await context.Decks.AddAsync(deck, cancellationToken);

    public Task RemoveAsync(Deck deck, CancellationToken cancellationToken = default)
    {
        context.Remove(deck);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Deck deck, CancellationToken cancellationToken = default)
    {
        context.Decks.Update(deck);
        return  Task.CompletedTask;
    }
}