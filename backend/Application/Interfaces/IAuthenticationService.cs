using Backend.Application.DTOs;

namespace Backend.Application.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<AuthResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
