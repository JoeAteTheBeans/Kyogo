using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication.Tokens;

public sealed class TokenExchangeService(IUserRepository userRepository, ITokenGeneratorService tokenGeneratorService, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
{
    public async Task<TokenExchangeResult> ExchangeTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        if (refreshToken.Revoked)
            return new TokenExchangeResult(TokenExchangeResultState.AlreadyRevoked, null, null);
        if (DateTime.UtcNow > refreshToken.Expires)
            return new TokenExchangeResult(TokenExchangeResultState.Expired, null, null);
        Task revokeTask = refreshTokenRepository.RevokeAsync(refreshToken, cancellationToken);
        Task<User?> userTask = userRepository.GetByIdAsync(refreshToken.UserId, cancellationToken);
        await Task.WhenAll(revokeTask, userTask);
        if (userTask.Result is null)
            return new TokenExchangeResult(TokenExchangeResultState.UserNotFound, null, null);
        RefreshToken newRefreshToken = tokenGeneratorService.GenerateRefreshToken(userTask.Result);
        await refreshTokenRepository.AddAsync(newRefreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new TokenExchangeResult(TokenExchangeResultState.Exchanged, newRefreshToken, tokenGeneratorService.GenerateToken(userTask.Result));
    }
}