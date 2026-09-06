using System.Data;
using System.Data.Common;
using Backend.Domain.Interfaces.Services;

namespace Backend.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork, IUnitOfWorkConnectionProvider, IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbTransaction? Transaction => _transaction;

    public async Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
    {
        _connection ??= _connectionFactory.CreateConnection();

        if (_connection.State != ConnectionState.Open)
        {
            if (_connection is DbConnection dbConnection)
                await dbConnection.OpenAsync(cancellationToken);
            else
                _connection.Open();
        }

        return _connection;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var connection = await GetConnectionAsync(cancellationToken);
        _transaction = connection.BeginTransaction();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
    }
}
