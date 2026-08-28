using Backend.Domain.Entities;
using Backend.Domain.Interfaces.Repositories;
using Backend.Infrastructure.Data;
using Dapper;

namespace Backend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                username AS Username,
                email AS Email,
                passwordHash AS PasswordHash,
                permissions AS Permissions,
                isLocked AS IsLocked,
                accessFailedCount AS AccessFailedCount
            FROM [kanannon].[dbo].[user]
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync<UserDbModel>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return row?.ToEntity();
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                username AS Username,
                email AS Email,
                passwordHash AS PasswordHash,
                permissions AS Permissions,
                isLocked AS IsLocked,
                accessFailedCount AS AccessFailedCount
            FROM [kanannon].[dbo].[user]
            WHERE username = @Username;
            """;

        using var connection = _connectionFactory.CreateConnection();
        var row = await connection.QuerySingleOrDefaultAsync<UserDbModel>(
            new CommandDefinition(sql, new { Username = username }, cancellationToken: cancellationToken));

        return row?.ToEntity();
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT
                id AS Id,
                name AS Name,
                username AS Username,
                email AS Email,
                passwordHash AS PasswordHash,
                permissions AS Permissions,
                isLocked AS IsLocked,
                accessFailedCount AS AccessFailedCount
            FROM [kanannon].[dbo].[user];
            """;

        using var connection = _connectionFactory.CreateConnection();
        var rows = await connection.QueryAsync<UserDbModel>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        return rows.Select(r => r.ToEntity()).ToList();
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO [kanannon].[dbo].[user]
            (
                id,
                name,
                username,
                email,
                passwordHash,
                permissions,
                isLocked,
                accessFailedCount
            )
            VALUES
            (
                @Id,
                @Name,
                @Username,
                @Email,
                @PasswordHash,
                @Permissions,
                @IsLocked,
                @AccessFailedCount
            );
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new
            {
                user.Id,
                user.Name,
                user.Username,
                user.Email,
                PasswordHash = user.PasswordHash.Value,
                user.Permissions,
                user.IsLocked,
                user.AccessFailedCount
            },
            cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE [kanannon].[dbo].[user]
            SET
                name = @Name,
                email = @Email,
                passwordHash = @PasswordHash,
                permissions = @Permissions,
                isLocked = @IsLocked,
                accessFailedCount = @AccessFailedCount
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new
            {
                user.Id,
                user.Name,
                user.Email,
                PasswordHash = user.PasswordHash.Value,
                user.Permissions,
                user.IsLocked,
                user.AccessFailedCount
            },
            cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        const string sql = """
            DELETE FROM [kanannon].[dbo].[user]
            WHERE id = @Id;
            """;

        using var connection = _connectionFactory.CreateConnection();
        await connection.ExecuteAsync(new CommandDefinition(
            sql,
            new { Id = id },
            cancellationToken: cancellationToken));
    }

    private sealed class UserDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Permissions { get; set; }
        public bool IsLocked { get; set; }
        public int AccessFailedCount { get; set; }

        public User ToEntity() => new(
            Id,
            Name,
            Username,
            Email,
            PasswordHash,
            Permissions,
            IsLocked,
            AccessFailedCount
        );
    }
}
