using Kyogo.Application.Persistence;
using Kyogo.Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class KanjiRepository(KyogoDbContext context) : IKanjiRepository
{
    public async Task<Kanji> GetAsync(KanjiId kanjiId, CancellationToken cancellationToken = default)
        => await context.Kanji.FirstOrDefaultAsync(x => x.Id == kanjiId, cancellationToken)
           ?? throw new KanjiNotFoundException(kanjiId);
}