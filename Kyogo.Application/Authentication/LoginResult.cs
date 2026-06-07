using Kyogo.Application.Authentication.Tokens;

namespace Kyogo.Application.Authentication;

public record LoginResult(LoginResultState State, Token? Token, RefreshToken? RefreshToken);