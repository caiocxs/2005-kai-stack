using Backend.Domain.Entities;
using Backend.Domain.Services;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Security;

public class AspNetPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(string plainPassword)
    {
        return _hasher.HashPassword(null!, plainPassword);
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, plainPassword);
        return result != PasswordVerificationResult.Failed;
    }
}
