namespace Kyogo.Application.Authentication.Tokens;

public record TokenExchangeResult(TokenExchangeResultState State, RefreshToken? RefreshToken, Token? Token);