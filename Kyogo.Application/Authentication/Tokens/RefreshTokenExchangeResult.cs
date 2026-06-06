namespace Kyogo.Application.Authentication.Tokens;

public record RefreshTokenExchangeResult(RefreshTokenExchangeResultState State, RefreshToken? RefreshToken, Token? Token);