using Kyogo.Domain.Characters;

namespace Kyogo.Application.Persistence;

public interface IKanjiRepository
{
    public Task<Kanji> GetAsync(KanjiId kanjiId, CancellationToken cancellationToken = default);
}