using Backend.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;

    public ServiceManager(IServiceProvider serviceProvider)
    {
        _authenticationService = new Lazy<IAuthenticationService>(serviceProvider.GetRequiredService<IAuthenticationService>);
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
