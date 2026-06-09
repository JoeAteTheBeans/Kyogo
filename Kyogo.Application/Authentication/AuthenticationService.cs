using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication;

public sealed class AuthenticationService(IUserRepository userRepository, ITokenGeneratorService tokenGeneratorService, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository)
{
    public async Task<RegisterResult> RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        Task<User?> userByUsernameTask = userRepository.GetByUsernameAsync(username, cancellationToken);
        Task<User?> userByEmailTask = userRepository.GetByEmailAsync(email, cancellationToken);
        await Task.WhenAll(userByUsernameTask, userByEmailTask);
        if (userByUsernameTask.Result is not null)
            return new RegisterResult(RegisterResultState.UsernameTaken, null, null);
        if (userByEmailTask.Result is not null)
            return new RegisterResult(RegisterResultState.EmailInUse, null, null);
        User registering = new User()
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
        };
        RefreshToken refreshToken = tokenGeneratorService.GenerateRefreshToken(registering);
        Task addUserTask = userRepository.AddAsync(registering, cancellationToken);
        Task addRefreshTokenTask = refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await Task.WhenAll(addUserTask, addRefreshTokenTask);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new RegisterResult(RegisterResultState.Success, tokenGeneratorService.GenerateToken(registering), refreshToken);
    }

    public async Task<LoginResult> LoginWithUsernameAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByUsernameAsync(username, cancellationToken);
        return user is null ? new LoginResult(LoginResultState.UsernameDoesNotExist, null, null) : await LoginAsync(user, password, cancellationToken);
    }

    public async Task<LoginResult> LoginWithEmailAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        User? user = await userRepository.GetByEmailAsync(email, cancellationToken);
        return user is null ? new LoginResult(LoginResultState.EmailDoesNotExist, null, null) : await LoginAsync(user, password, cancellationToken);
    }

    private async Task<LoginResult> LoginAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        if (!BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash))
            return new LoginResult(LoginResultState.IncorrectPassword, null, null);

        RefreshToken refreshToken = tokenGeneratorService.GenerateRefreshToken(user);
        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new LoginResult(LoginResultState.Success, tokenGeneratorService.GenerateToken(user), refreshToken);
    }
}