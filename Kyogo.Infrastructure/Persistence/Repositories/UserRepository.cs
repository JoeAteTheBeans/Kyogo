using Kyogo.Application.Persistence;
using Kyogo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Kyogo.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(KyogoDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        =>  await context.Users.FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        => await context.Users.AddAsync(user, cancellationToken);

}