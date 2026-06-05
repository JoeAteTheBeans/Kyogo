using Kyogo.Application.Persistence;
using Kyogo.Application.SpacedRepetition;
using Kyogo.Domain.Decks;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Study;

public sealed class StudyRegimentService(IDeckRepository deckRepository, IDeckSubscriptionRepository deckSubscriptionRepository, IProgressRepository progressRepository)
{
    public async Task<IReadOnlyCollection<TermId>> GetTermsAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        Task<IReadOnlyCollection<DeckSubscription>> subscriptionsTask = deckSubscriptionRepository.GetAllByUserAsync(userId, cancellationToken);
        Task<IReadOnlyCollection<Progress>> progressTask = progressRepository.GetAllByUserAsync(userId, cancellationToken);
        await Task.WhenAll(subscriptionsTask, progressTask);
        var decksTask = deckRepository.GetManyAsync(subscriptionsTask.Result.Select(x => x.DeckId), cancellationToken);
        var subDecksTask = deckRepository.GetSubDecksOfManyAsync(subscriptionsTask.Result.Select(x => x.DeckId), cancellationToken);
        await Task.WhenAll(subDecksTask, decksTask);
        List<Deck> decks = decksTask.Result.ToList();
        decks.AddRange(subDecksTask.Result);
        HashSet<TermId> terms = [];
        foreach (Deck deck in decks)
            terms.UnionWith(deck.Terms);
        terms.UnionWith(progressTask.Result.Select(x => ProgressId.DeriveTermId(x.Id)));
        return terms;
    }

    public async Task<IReadOnlyCollection<TermId>> GetNewTermsAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var termsTask = GetTermsAsync(userId, cancellationToken);
        Task<IReadOnlyCollection<Progress>> progressTask = progressRepository.GetAllByUserAsync(userId, cancellationToken);
        await Task.WhenAll(termsTask, progressTask);
        return termsTask.Result.Except(progressTask.Result.Select(x => ProgressId.DeriveTermId(x.Id))).ToList();
    }
}