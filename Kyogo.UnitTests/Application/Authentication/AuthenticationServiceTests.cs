using Kyogo.Application.Authentication;
using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;
using Moq;

namespace Kyogo.UnitTests.Application.Authentication;

public sealed class AuthenticationServiceTests
{
    private readonly AuthenticationService _authenticationService;
    
    private readonly Mock<IUserRepository> _mockUserRepository = new();
    private readonly Mock<ITokenGeneratorService> _mockTokenGeneratorService = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository = new();

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

    public AuthenticationServiceTests()
        => _authenticationService =  new AuthenticationService(_mockUserRepository.Object, _mockTokenGeneratorService.Object, _mockUnitOfWork.Object, _mockRefreshTokenRepository.Object);
    
    [Fact]
    public async Task RegisterAsync_UsernameTaken_ReturnsUsernameTakenState()
    {
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnyUser);
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        RegisterResult result = await _authenticationService.RegisterAsync(string.Empty, string.Empty, string.Empty);
        Assert.Equal(RegisterResultState.UsernameTaken, result.State);  
    }

    [Fact]
    public async Task RegisterAsync_EmailInUse_ReturnsEmailInUseState()
    {
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AnyUser);
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        RegisterResult result = await _authenticationService.RegisterAsync(string.Empty, string.Empty,  string.Empty);
        Assert.Equal(RegisterResultState.EmailInUse, result.State);
    }

    [Fact]
    public async Task RegisterAsync_UniqueCredentials_ReturnsSuccessStateWithTokens()
    {
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        _mockTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(default(Token));
        _mockTokenGeneratorService
            .Setup(x => x.GenerateRefreshToken(It.IsAny<User>()))
            .Returns(AnyRefreshToken);
        RegisterResult result = await _authenticationService.RegisterAsync(string.Empty, string.Empty, string.Empty);
        Assert.Equal(RegisterResultState.Success, result.State);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task LoginWithUsername_UsernameDoesNotExist_ReturnsUsernameDoesNotExistState()
    {
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        LoginResult result = await _authenticationService.LoginWithUsernameAsync(string.Empty, string.Empty);
        Assert.Equal(LoginResultState.UsernameDoesNotExist, result.State);
    }

    [Fact]
    public async Task LoginWithUsername_CorrectCredentials_ReturnsSuccessStateWithTokens()
    {
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Id = default,
                Username = string.Empty,
                Email = string.Empty,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("correct_password")
            });
        _mockTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(default(Token));
        _mockTokenGeneratorService
            .Setup(x => x.GenerateRefreshToken(It.IsAny<User>()))
            .Returns(AnyRefreshToken);
        LoginResult result = await _authenticationService.LoginWithUsernameAsync(string.Empty, "correct_password");
        Assert.Equal(LoginResultState.Success, result.State);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }
    
    [Fact]
    public async Task LoginWithEmail_EmailDoesNotExist_ReturnsEmailDoesNotExistState()
    {
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);
        LoginResult result = await _authenticationService.LoginWithEmailAsync(string.Empty, string.Empty);
        Assert.Equal(LoginResultState.EmailDoesNotExist, result.State);
    }

    [Fact]
    public async Task LoginWithEmail_CorrectCredentials_ReturnsSuccessStateWithTokens()
    {
        _mockUserRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Id = default,
                Username = string.Empty,
                Email = string.Empty,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("correct_password")
            });
        _mockTokenGeneratorService
            .Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(default(Token));
        _mockTokenGeneratorService
            .Setup(x => x.GenerateRefreshToken(It.IsAny<User>()))
            .Returns(AnyRefreshToken);
        LoginResult result = await _authenticationService.LoginWithEmailAsync(string.Empty, "correct_password");
        Assert.Equal(LoginResultState.Success, result.State);
        Assert.NotNull(result.Token);
        Assert.NotNull(result.RefreshToken);
    }

    [Fact]
    public async Task LoginAsync_IncorrectPassword_ReturnsIncorrectPasswordState()
    {
        //Could also test via the LoginWithEmailAsync method. Both filter into LoginAsync.
        _mockUserRepository
            .Setup(x => x.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User
            {
                Id = default,
                Username = string.Empty,
                Email = string.Empty,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("correct_password")
            });
        LoginResult result = await _authenticationService.LoginWithUsernameAsync(string.Empty, "incorrect_password");
        Assert.Equal(LoginResultState.IncorrectPassword, result.State);
    }
}