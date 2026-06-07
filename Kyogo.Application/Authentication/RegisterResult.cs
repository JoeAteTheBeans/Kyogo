using Kyogo.Application.Authentication.Tokens;

namespace Kyogo.Application.Authentication;

public record RegisterResult(RegisterResultState State, Token? Token, RefreshToken? RefreshToken);