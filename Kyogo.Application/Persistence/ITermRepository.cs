using Kyogo.Domain.Vocabulary;

namespace Kyogo.Application.Persistence;

public interface ITermRepository
{
    public Task<Term> GetAsync(TermId termId, CancellationToken cancellationToken = default);
}