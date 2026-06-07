namespace Kyogo.Infrastructure.Authentication;

public sealed class JwtSettings
{
    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int TokenExpiration { get; init; } = 60;
    public int RefreshTokenExpiration { get; init; } = 10080;
}