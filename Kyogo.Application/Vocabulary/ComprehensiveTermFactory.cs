using Kyogo.Application.Persistence;
using Kyogo.Application.SpacedRepetition;
using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Vocabulary;

public sealed class ComprehensiveTermFactory(ITermRepository termRepository, ITermModificationRepository termModificationRepository, IProgressRepository progressRepository)
{
    public async Task<ComprehensiveTerm> GetAsync(UserId userId, TermId termId,
        CancellationToken cancellationToken = default)
    {
        Task<Term> termTask = termRepository.GetAsync(termId, cancellationToken);
        Task<TermModification?> termModificationTask = termModificationRepository.GetAsync(userId, termId, cancellationToken);
        Task<IReadOnlyCollection<Progress>> progressTask = progressRepository.GetAllByUserByTermAsync(userId, termId, cancellationToken);
        await Task.WhenAll(termTask, termModificationTask, progressTask);
        return new ComprehensiveTerm(termTask.Result, termModificationTask.Result, progressTask.Result);
    }
}