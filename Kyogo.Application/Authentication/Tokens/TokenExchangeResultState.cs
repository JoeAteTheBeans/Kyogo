namespace Kyogo.Application.Authentication.Tokens;

public enum TokenExchangeResultState
{
    RefreshTokenNotFound,
    Revoked,
    Expired,
    UserNotFound,
    Exchanged
}