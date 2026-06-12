using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;
using Moq;

namespace Kyogo.UnitTests.Application.Authentication;

public sealed class TokenExchangeServiceTests
{
    private readonly TokenExchangeService _tokenExchangeService;

    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<ITokenGeneratorService> _mockTokenGeneratorService = new();
    private readonly Mock<IRefreshTokenRepository>  _mockRefreshTokenRepository = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    
    private static readonly User AnyUser = new User
    {
        Id = default,
        Username = string.Empty,
        Email = string.Empty,
        PasswordHash = string.Empty
    };

    private static readonly RefreshToken AnyRefreshToken = new RefreshToken
    {
        Id = default,
        RawToken = string.Empty,
        UserId = default,
        Created = default,
        Expires = default,
        Revoked = false
    };
    
    private static RefreshToken GetValidRefreshToken()
        => new RefreshToken
        {
            Id = default,
            RawToken = string.Empty,
            UserId = default,
            Created = default,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Revoked = false
        };

    public TokenExchangeServiceTests()
        => _tokenExchangeService = new TokenExchangeService(_mockUserRepository.Object, _mockTokenGeneratorService.Object, _mockRefreshTokenRepository.Object, _mockUnitOfWork.Object);

    [Fact]
    public async Task ExchangeTokenAsync_RefreshTokenRevoked_ReturnsRevokedState()
    {
        TokenExchangeResult result = await _tokenExchangeService.ExchangeTokenAsync(new RefreshToken
        {
            Id = default,
            RawToken = string.Empty,
            UserId = default,
            Created = default,
            Expires = default,
            Revoked = true
        });
        Assert.Equal(TokenExchangeResultState.Revoked, result.State);
    }

    [Fact]
    public async Task ExchangeTokenAsync_RefreshTokenExpired_ReturnsExpiredState()
    {
        TokenExchangeResult result = await _tokenExchangeService.ExchangeTokenAsync(new RefreshToken
        {
            Id = default,
            RawToken = string.Empty,
            UserId = default,
            Created = default,
            Expires = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1)),
            Revoked = false
        });
        Assert.Equal(TokenExchangeResultState.Expired, result.State);
    }

    [Fact]
    public async Task ExchangeTokenAsync_UserNotFound_ReturnsNotFoundState()
    {
        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        TokenExchangeResult result = await _tokenExchangeService.ExchangeTokenAsync(GetValidRefreshToken());
        Assert.Equal(TokenExchangeResultState.UserNotFound, result.State);
    }

    [Fact]
    public async Task ExchangeTokenAsync_ValidToken_ReturnsExchangedStateAndTokens()
    {
        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnyUser);
        _mockTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(default(Token));
        _mockTokenGeneratorService
            .Setup(x => x.GenerateRefreshToken(It.IsAny<User>()))
            .Returns(AnyRefreshToken);
        TokenExchangeResult result = await _tokenExchangeService.ExchangeTokenAsync(GetValidRefreshToken());
        Assert.Equal(TokenExchangeResultState.Exchanged, result.State);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task ExchangeByRawTokenAsync_RefreshTokenNotFound_ReturnsRefreshTokenNotFoundState()
    {
        _mockRefreshTokenRepository
            .Setup(x => x.GetByRawTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as RefreshToken);
        TokenExchangeResult result = await _tokenExchangeService.ExchangeByRawTokenAsync(string.Empty);
        Assert.Equal(TokenExchangeResultState.RefreshTokenNotFound, result.State);
    }
    
    [Fact]
    public async Task ExchangeByRawTokenAsync_ValidRawToken_ReturnsExchangedStateAndTokens()
    {
        _mockRefreshTokenRepository
            .Setup(x => x.GetByRawTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(GetValidRefreshToken());
        _mockUserRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<UserId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnyUser);
        _mockTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(default(Token));
        _mockTokenGeneratorService
            .Setup(x => x.GenerateRefreshToken(It.IsAny<User>()))
            .Returns(AnyRefreshToken);
        TokenExchangeResult result = await _tokenExchangeService.ExchangeByRawTokenAsync(string.Empty);
        Assert.Equal(TokenExchangeResultState.Exchanged, result.State);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }
}