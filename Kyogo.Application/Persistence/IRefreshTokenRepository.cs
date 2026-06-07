using Kyogo.Application.Authentication.Tokens;

namespace Kyogo.Application.Persistence;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetAsync(RefreshTokenId id, CancellationToken cancellationToken = default);
    public Task<RefreshToken?> GetByRawTokenAsync(string rawToken, CancellationToken cancellationToken = default);
    public Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    public Task RevokeAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}