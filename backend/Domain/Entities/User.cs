using Backend.Domain.Services;
using Backend.Domain.Utilities;
using Microsoft.Data.SqlClient;

namespace Backend.Domain.Entities;

public class User
{
  public readonly Guid Id;
  public readonly string Name;
  public readonly string Username;
  public readonly string Email;
  public PasswordHash PasswordHash { get; private set; }
  public readonly int Permissions;
  public bool IsLocked { get; private set; }
  public int AccessFailedCount { get; private set; }

  private User() { }

  public User(string email, string plainPassword, IPasswordHasher hasher)
  {
    Email = email.ToLowerInvariant().Trim();
    SetPassword(plainPassword, hasher);
  }

  public User(Guid id, string name, string username, string email, string password, int permissions)
  {
    Id = id;
    Name = name;
    Username = username;
    Email = email;
    PasswordHash = new PasswordHash(password);
    Permissions = permissions;
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

  public static User? ConsultUser(string username)
  {
    string query = @"
        SELECT
          id,
          name,
          email,
          passwordHash,
          admin
        FROM [Kanannon].[dbo].[User]
        WHERE username = @username;";

    User? user = null;

    using (SqlConnection cn = ConnectionFactory.ConnectDatabase())
    {
      var cmd = cn.CreateCommand();

      cmd.CommandText = query;
      cmd.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 50).Value = username;
      var dr = cmd.ExecuteReader();

      if (dr.Read())
      {
        string id = dr.GetString(dr.GetOrdinal("id"));
        string name = dr.GetString(dr.GetOrdinal("name"));
        string email = dr.GetString(dr.GetOrdinal("email"));
        bool permissions = dr.GetBoolean(dr.GetOrdinal("admin"));
        string passwordHash = dr.GetString(dr.GetOrdinal("passwordHash"));

        user = new User(Guid.Parse(id), name, username, email, passwordHash, permissions ? 1 : 0);
      }
    }

    return user;
  }
}
