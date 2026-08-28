using Backend.Application.Interfaces;
using Backend.Application.Mappings;
using Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(UserProfile).Assembly));
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
