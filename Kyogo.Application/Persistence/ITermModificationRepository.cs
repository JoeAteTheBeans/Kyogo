using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public interface ITermModificationRepository
{
    public Task<TermModification?> GetAsync(UserId userId, TermId termId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<TermModification>> GetAllByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    public Task AddAsync(TermModification modification,  CancellationToken cancellationToken = default);
    public Task RemoveAsync(TermModification modification, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TermModification modification, CancellationToken cancellationToken = default);
}