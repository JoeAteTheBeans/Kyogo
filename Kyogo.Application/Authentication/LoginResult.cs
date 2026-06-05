using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication;

public record LoginResult(LoginResultState State, Token? Token);