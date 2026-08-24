namespace Backend.Domain.Utilities;

public sealed class PasswordHash : IEquatable<PasswordHash>
{
  public string Value { get; }

  public PasswordHash(string value)
  {
    if (string.IsNullOrWhiteSpace(value))
      throw new ArgumentException("The password hash cannot be null.");

    Value = value;
  }
  public static PasswordHash Create(string hash) => new(hash);

  public bool Equals(PasswordHash? other) => other is not null && Value == other.Value;

  public override bool Equals(object? obj) => obj is PasswordHash other && Equals(other);

  public override int GetHashCode() => Value.GetHashCode();
}
