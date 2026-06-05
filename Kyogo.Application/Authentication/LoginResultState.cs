namespace Kyogo.Application.Authentication;

public enum LoginResultState
{
    UsernameDoesNotExist,
    EmailDoesNotExist,
    IncorrectPassword,
    Success
}