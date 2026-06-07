using Kyogo.Application.Persistence;
using Kyogo.Domain.Vocabulary;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class TermRepository(KyogoDbContext context) : ITermRepository
{
    public async Task<Term> GetAsync(TermId termId, CancellationToken cancellationToken = default)
        => await context.Terms.FirstOrDefaultAsync(x => x.Id == termId, cancellationToken) 
           ?? throw new TermNotFoundException(termId);
}