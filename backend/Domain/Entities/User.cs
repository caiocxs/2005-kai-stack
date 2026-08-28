using Backend.Domain.Services;
using Backend.Domain.Utilities;

namespace Backend.Domain.Entities;

public class User : BaseEntity
{
  public string Name { get; private set; } = string.Empty;
  public string Username { get; private set; } = string.Empty;
  public string Email { get; private set; } = string.Empty;
  public PasswordHash PasswordHash { get; private set; } = default!;
  public int Permissions { get; private set; }
  public bool IsLocked { get; private set; }
  public int AccessFailedCount { get; private set; }

  private User() { }

  public User(
      Guid id,
      string name,
      string username,
      string email,
      string passwordHash,
      int permissions,
      bool isLocked = false,
      int accessFailedCount = 0)
  {
    Id = id;
    Name = name;
    Username = username;
    Email = email.ToLowerInvariant().Trim();
    PasswordHash = new PasswordHash(passwordHash);
    Permissions = permissions;
    IsLocked = isLocked;
    AccessFailedCount = accessFailedCount;
  }

  public static User Create(
      string name,
      string username,
      string email,
      string plainPassword,
      int permissions,
      IPasswordHasher hasher)
  {
    ValidatePasswordStrength(plainPassword);
    var hash = hasher.HashPassword(plainPassword);
    return new User(Guid.NewGuid(), name, username, email, hash, permissions);
  }

  public void SetPassword(string plainPassword, IPasswordHasher hasher)
  {
    ValidatePasswordStrength(plainPassword);
    var hash = hasher.HashPassword(plainPassword);
    PasswordHash = PasswordHash.Create(hash);
  }

  public bool Authenticate(string plainPassword, IPasswordHasher hasher)
  {
    if (IsLocked)
      throw new UnauthorizedAccessException("Account is blocked by multiple wrong attempts.");

    bool isValid = hasher.VerifyPassword(plainPassword, PasswordHash.Value);

    if (isValid)
    {
      AccessFailedCount = 0;
    }
    else
    {
      AccessFailedCount++;
      if (AccessFailedCount >= 3)
        IsLocked = true;
    }

    return isValid;
  }

  private static void ValidatePasswordStrength(string plainPassword)
  {
    if (string.IsNullOrWhiteSpace(plainPassword) || plainPassword.Length < 8)
      throw new ArgumentException("Password must contain at least 8 characters.");
  }
}
