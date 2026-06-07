using Kyogo.Application.Persistence;
using Kyogo.Application.SpacedRepetition;
using Kyogo.Domain.Users;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class ProgressRepository(KyogoDbContext context) : IProgressRepository
{
    public async Task<Progress?> GetAsync(ProgressId progressId, CancellationToken cancellationToken = default)
        => await context.ProgressRecords.FirstOrDefaultAsync(x => x.Id == progressId, cancellationToken);

    public async Task<IReadOnlyCollection<Progress>> GetAllByUserAsync(UserId ownerId, CancellationToken cancellationToken = default)
        => await context.ProgressRecords.Where(x => x.OwnerId == ownerId).ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<Progress>> GetAllByUserByTermAsync(UserId ownerId, TermId termId, CancellationToken cancellationToken = default)
        => await context.ProgressRecords.Where(x => x.OwnerId == ownerId && x.TermId == termId).ToListAsync(cancellationToken);

    public async Task<Progress?> GetByUserByTermByCardTypeAsync(UserId ownerId, TermId termId, CardType cardType, CancellationToken cancellationToken = default)
        => await context.ProgressRecords.FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.TermId == termId && x.CardType == cardType, cancellationToken);

    public async Task AddAsync(Progress progress, CancellationToken cancellationToken = default)
        =>  await context.ProgressRecords.AddAsync(progress, cancellationToken);

    public Task RemoveAsync(Progress progress, CancellationToken cancellationToken = default)
    {
        context.ProgressRecords.Remove(progress);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default)
    {
        context.ProgressRecords.Update(progress);
        return Task.CompletedTask;
    }
}