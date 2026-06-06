using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;

namespace Kyogo.Application.Authentication;

public sealed class AuthenticationService(IUserRepository userRepository, ITokenService tokenService, IUnitOfWork unitOfWork)
{
    public async Task<RegisterResult> RegisterAsync(string username, string email, string password, CancellationToken cancellationToken = default)
    {
        Task<User?> userByUsername = userRepository.GetByUsernameAsync(username, cancellationToken);
        Task<User?> userByEmail = userRepository.GetByEmailAsync(email, cancellationToken);
        await Task.WhenAll(userByUsername, userByEmail);
        if (userByUsername.Result is not null)
            return new RegisterResult(RegisterResultState.UsernameTaken, null);
        if (userByEmail.Result is not null)
            return new RegisterResult(RegisterResultState.EmailInUse, null);
        User registering = new User()
        {
            Id = new UserId(Guid.NewGuid()),
            Username = username,
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(password)
        };
        await userRepository.AddAsync(registering, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return new RegisterResult(RegisterResultState.Success, tokenService.GenerateToken(registering));
    }

    public async Task<LoginResult> LoginWithUsernameAsync(string username, string password, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByUsernameAsync(username, cancellationToken);
        return user is null ? new LoginResult(LoginResultState.UsernameDoesNotExist, null) : Login(user, password);
    }

    public async Task<LoginResult> LoginWithEmailAsync(string email, string password, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailAsync(email, cancellationToken);
        return user is null ? new LoginResult(LoginResultState.EmailDoesNotExist, null) : Login(user, password);
    }

    private LoginResult Login(User user, string password)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, user.PasswordHash) ? 
            new LoginResult(LoginResultState.Success, tokenService.GenerateToken(user)) :
            new LoginResult(LoginResultState.IncorrectPassword, null);
}