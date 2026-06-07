using Kyogo.Application.Persistence;
using Kyogo.Application.Vocabulary.Modifications;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class TermModificationRepository(KyogoDbContext context) : ITermModificationRepository
{
    public async Task<TermModification?> GetAsync(UserId ownerId, TermId termId, CancellationToken cancellationToken = default)
        => await context.TermModifications.FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.TermId == termId, cancellationToken);

    public async Task<IReadOnlyCollection<TermModification>> GetAllByOwnerAsync(UserId ownerId, CancellationToken cancellationToken = default)
        => await context.TermModifications.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);
    
    public async Task AddAsync(TermModification modification, CancellationToken cancellationToken = default)
        => await context.TermModifications.AddAsync(modification, cancellationToken);

    public Task RemoveAsync(TermModification modification, CancellationToken cancellationToken = default)
    {
        context.TermModifications.Remove(modification);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TermModification modification, CancellationToken cancellationToken = default)
    {
        context.TermModifications.Update(modification);
        return Task.CompletedTask;
    }
}