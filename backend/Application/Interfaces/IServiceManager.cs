namespace Backend.Application.Interfaces;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
}
