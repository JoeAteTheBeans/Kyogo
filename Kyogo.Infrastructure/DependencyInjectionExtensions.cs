using Kyogo.Application.Authentication.Tokens;
using Kyogo.Application.Persistence;
using Kyogo.Infrastructure.Authentication;
using Kyogo.Infrastructure.Persistence;
using Kyogo.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kyogo.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<KyogoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IDeckSubscriptionRepository, DeckSubscriptionRepository>();
        services.AddScoped<IKanjiRepository, KanjiRepository>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ITermModificationRepository, TermModificationRepository>();
        services.AddScoped<ITermRepository, TermRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<ITokenGeneratorService, JwtTokenGeneratorService>();
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        return services;
    }
}