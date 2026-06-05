using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;

namespace Kyogo.Application.Persistence;

public interface IDeckSubscriptionRepository
{
    public Task<IReadOnlyCollection<DeckSubscription>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    public Task AddAsync(DeckSubscription subscription, CancellationToken cancellationToken = default);
    public Task RemoveAsync(DeckSubscription subscription, CancellationToken cancellationToken = default);
}