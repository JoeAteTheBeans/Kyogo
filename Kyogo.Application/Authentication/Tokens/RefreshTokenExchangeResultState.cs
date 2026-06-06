namespace Kyogo.Application.Authentication.Tokens;

public enum RefreshTokenExchangeResultState
{
    AlreadyRevoked,
    Expired,
    UserNotFound,
    Exchanged
}