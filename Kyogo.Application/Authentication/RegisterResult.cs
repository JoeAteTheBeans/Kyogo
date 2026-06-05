using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication;

public record RegisterResult(RegisterResultState State, Token? Token);