using Backend.Domain.Entities;

namespace Backend.Application.Interfaces;

public interface ITokenService
{
    (string Token, DateTimeOffset ExpiresAt) GenerateToken(User user);
}
