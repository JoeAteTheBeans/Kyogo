using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kyogo.Application.Authentication.Tokens;
using Kyogo.Domain.Users;
using Kyogo.Infrastructure.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Kyogo.UnitTests.Infrastructure.Authentication;

public sealed class JwtTokenGeneratorServiceTests
{
    private readonly JwtTokenGeneratorService _jwtTokenGeneratorService;

    private readonly JwtSettings _jwtSettings = new ()
    {
        Secret = "this-is-a-test-secret-key-123456789",
        Issuer = "issuer",
        Audience = "audience",
        TokenExpiration = 1,
        RefreshTokenExpiration = 10,
    };
    
    private readonly User _user = new ()
    {
        Id = new UserId(Guid.NewGuid()),
        Username = string.Empty,
        Email = string.Empty,
        PasswordHash = string.Empty
    };

    private const int AcceptableTickVariance = 10000000;
    
    public JwtTokenGeneratorServiceTests()
        => _jwtTokenGeneratorService = new JwtTokenGeneratorService(Options.Create(_jwtSettings));

    [Fact]
    public void GenerateToken_ReturnsValidToken()
    {
        Token generatedToken = _jwtTokenGeneratorService.GenerateToken(_user);
        DateTime expectedExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpiration);
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        TokenValidationParameters validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true
        };
        ClaimsPrincipal principal = handler.ValidateToken(generatedToken.Value, validationParameters, out _);
        Assert.NotNull(principal);
        JwtSecurityToken decodedToken = handler.ReadJwtToken(generatedToken.Value);
        Assert.Equal(_user.Id.Value.ToString(), decodedToken.Subject);
        Assert.True(Guid.TryParse(decodedToken.Id, out Guid _));
        Assert.Equal(_jwtSettings.Issuer, decodedToken.Issuer);
        Assert.Contains(_jwtSettings.Audience, decodedToken.Audiences);
        Assert.Single(decodedToken.Audiences);
        Assert.True(Math.Abs(expectedExpiry.Ticks - decodedToken.ValidTo.Ticks) < AcceptableTickVariance);
    }

    [Fact]
    public void GenerateToken_ReturnsUniqueTokens()
    {
        Token generatedTokenA = _jwtTokenGeneratorService.GenerateToken(_user);
        Token generatedTokenB = _jwtTokenGeneratorService.GenerateToken(_user);
        Assert.NotEqual(generatedTokenA, generatedTokenB);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsValidToken()
    {
        RefreshToken generatedToken = _jwtTokenGeneratorService.GenerateRefreshToken(_user);
        DateTime expectedCreation = DateTime.UtcNow;
        DateTime expectedExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiration);
        Assert.False(string.IsNullOrEmpty(generatedToken.RawToken));
        Assert.Equal(_user.Id, generatedToken.UserId);
        Assert.True(Math.Abs(expectedCreation.Ticks - generatedToken.Created.Ticks) < AcceptableTickVariance);
        Assert.True(Math.Abs(expectedExpiry.Ticks - generatedToken.Expires.Ticks) < AcceptableTickVariance);
        Assert.False(generatedToken.Revoked);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsUniqueTokens()
    {
        RefreshToken generatedTokenA = _jwtTokenGeneratorService.GenerateRefreshToken(_user);
        RefreshToken generatedTokenB = _jwtTokenGeneratorService.GenerateRefreshToken(_user);
        Assert.NotEqual(generatedTokenA.Id, generatedTokenB.Id);
        Assert.NotEqual(generatedTokenA.RawToken, generatedTokenB.RawToken);
    }
}