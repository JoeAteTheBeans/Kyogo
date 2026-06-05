using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication;

public interface ITokenService
{
    public Token GenerateToken(User user);
}