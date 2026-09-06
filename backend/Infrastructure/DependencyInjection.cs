using Backend.Application.Interfaces;
using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Backend.Domain.Services;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Repositories;
using Backend.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<UnitOfWork>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UnitOfWork>());
        services.AddScoped<IUnitOfWorkConnectionProvider>(sp => sp.GetRequiredService<UnitOfWork>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddSingleton<IPasswordHasher, AspNetPasswordHasher>();
        services.AddSingleton<ITokenService, JwtTokenService>();

        return services;
    }
}
