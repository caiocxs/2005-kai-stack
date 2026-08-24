using Backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastucture.Security
{
  public class AspNetPasswordHasher
  {
    private readonly PasswordHasher<User> _hasher = new();

    public string HashPassword(string plainPassword)
    {
      // aplica Salt + PBKDF2 com HMAC-SHA512
      return _hasher.HashPassword(null!, plainPassword);
    }

    public bool VerifyPassword(string plainPassword, string hashedPassword)
    {
      var result = _hasher.VerifyHashedPassword(null!, hashedPassword, plainPassword);
      return result != PasswordVerificationResult.Failed;
    }
  }
}
