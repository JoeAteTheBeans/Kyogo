using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class RefreshTokenRepository(KyogoDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetAsync(RefreshTokenId id, CancellationToken cancellationToken = default)
        => await context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<RefreshToken?> GetByRawTokenAsync(string rawToken, CancellationToken cancellationToken = default)
        => await context.RefreshTokens.FirstOrDefaultAsync(x => x.RawToken == rawToken, cancellationToken);

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        => await context.RefreshTokens.AddAsync(refreshToken, cancellationToken);

    public Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        context.Entry(refreshToken).Property(x => x.Revoked).CurrentValue = true;
        context.Entry(refreshToken).Property(x => x.Revoked).IsModified = true;
        return Task.CompletedTask;
    }
}