using Kyogo.Application.SpacedRepetition;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public interface IProgressRepository
{
    public Task<Progress?> GetAsync(UserId userId, ProgressId progressId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Progress>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<Progress>> GetAllByUserByTermAsync(UserId userId, TermId termId, CancellationToken cancellationToken = default);
    public Task AddAsync(Progress progress,  CancellationToken cancellationToken = default);
    public Task RemoveAsync(Progress progress, CancellationToken cancellationToken = default);
    public Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default);
}