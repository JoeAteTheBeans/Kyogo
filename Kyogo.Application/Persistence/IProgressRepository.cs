using Kyogo.Application.SpacedRepetition;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public interface IProgressRepository
{
    public Task<Progress?> GetAsync(ProgressId progressId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Progress>> GetAllByUserAsync(UserId ownerId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Progress>> GetAllByUserByTermAsync(UserId ownerId, TermId termId, CancellationToken cancellationToken = default);
    public Task<Progress?> GetByUserByTermByCardTypeAsync(UserId ownerId, TermId termId, CardType cardType, CancellationToken cancellationToken = default);
    public Task AddAsync(Progress progress,  CancellationToken cancellationToken = default);
    public Task RemoveAsync(Progress progress, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default);
}