using Kyogo.Domain.Users;

namespace Kyogo.Application.Persistence;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    public Task AddAsync(User user, CancellationToken cancellationToken = default);
}