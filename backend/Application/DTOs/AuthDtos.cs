namespace Backend.Application.DTOs;

public record LoginRequest(string Username, string Password);

public record LoginWithEmailRequest(string Email, string Password);

public record RegisterRequest(
    string Name,
    string Username,
    string Email,
    string Password,
    int Permissions
);

public record UserDto(
    Guid Id,
    string Name,
    string Username,
    string Email,
    int Permissions
);

public record UserSummaryDto(
    Guid Id,
    string Name,
    string Username
);

public record UserDetailDto(
    Guid Id,
    string Name,
    string Username,
    string Email,
    int Permissions,
    bool IsLocked,
    int AccessFailedCount
);

public record AuthResult(
    bool Success,
    string? Message = null,
    UserDto? User = null,
    string? Token = null,
    DateTimeOffset? ExpiresAt = null
);
