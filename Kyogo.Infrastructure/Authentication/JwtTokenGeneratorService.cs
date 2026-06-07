using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Kyogo.Application.Authentication.Tokens;
using Kyogo.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kyogo.Infrastructure.Authentication;

public sealed class JwtTokenGeneratorService(IOptions<JwtSettings> settings) : ITokenGeneratorService
{
    public Token GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Value.Issuer,
            audience: settings.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(settings.Value.TokenExpiration),
            signingCredentials: credentials
        );

        return new Token(new JwtSecurityTokenHandler().WriteToken(token));
    }

    public RefreshToken GenerateRefreshToken(User user)
        => new()
        {
            Id = new RefreshTokenId(Guid.NewGuid()),
            RawToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UserId = user.Id,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(settings.Value.RefreshTokenExpiration),
            Revoked = false
        };
}