using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public interface ITermModificationRepository
{
    public Task<TermModification?> GetAsync(UserId ownerId, TermId termId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<TermModification>> GetAllByOwnerAsync(UserId ownerId, CancellationToken cancellationToken = default);
    public Task AddAsync(TermModification modification,  CancellationToken cancellationToken = default);
    public Task RemoveAsync(TermModification modification, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TermModification modification, CancellationToken cancellationToken = default);
}