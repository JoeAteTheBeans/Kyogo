using Kyogo.Application.Authentication;
using Kyogo.Application.Authentication.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Kyogo.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<AuthenticationService>();
        services.AddScoped<TokenExchangeService>();
        return services;
    }
}