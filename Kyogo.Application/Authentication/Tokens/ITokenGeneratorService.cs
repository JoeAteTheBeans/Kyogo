using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication.Tokens;

public interface ITokenGeneratorService
{
    public Token GenerateToken(User user);
    public RefreshToken GenerateRefreshToken(User user);
}