using System.Data;

namespace Backend.Infrastructure.Data;

public interface IUnitOfWorkConnectionProvider
{
    Task<IDbConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    IDbTransaction? Transaction { get; }
}
