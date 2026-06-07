using Kyogo.Application.Persistence;

namespace Kyogo.Infrastructure.Persistence;

public class UnitOfWork(KyogoDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}