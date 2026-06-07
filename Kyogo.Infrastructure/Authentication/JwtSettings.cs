namespace Kyogo.Infrastructure.Authentication;

public sealed class JwtSettings
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int TokenExpiration { get; init; }
    public required int RefreshTokenExpiration { get; init; }
}