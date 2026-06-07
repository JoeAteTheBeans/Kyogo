using Kyogo.Application.Persistence;
using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class DeckSubscriptionRepository(KyogoDbContext context) : IDeckSubscriptionRepository
{
    public async Task<IReadOnlyCollection<DeckSubscription>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default)
        => await context.DeckSubscriptions.Where(x => x.UserId == userId).ToListAsync(cancellationToken);

    public async Task AddAsync(DeckSubscription subscription, CancellationToken cancellationToken = default)
        => await context.DeckSubscriptions.AddAsync(subscription, cancellationToken);

    public Task RemoveAsync(DeckSubscription subscription, CancellationToken cancellationToken = default)
    {
        context.DeckSubscriptions.Remove(subscription);
        return Task.CompletedTask;
    }
}