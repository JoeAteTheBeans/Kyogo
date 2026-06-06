using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication.Tokens;

public sealed class RefreshToken
{
    public required RefreshTokenId Id { get; init; }
    public required string RawToken { get; init; }
    public required UserId UserId { get; init; }
    public required DateTime Created  { get; init; }
    public required DateTime Expires { get; init; }
    public required bool Revoked { get; init; }
}