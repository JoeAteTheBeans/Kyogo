namespace Kyogo.Application.Authentication.Tokens;

public enum TokenExchangeResultState
{
    AlreadyRevoked,
    Expired,
    UserNotFound,
    Exchanged
}