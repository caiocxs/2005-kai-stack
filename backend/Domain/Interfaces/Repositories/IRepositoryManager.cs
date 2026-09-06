using Backend.Domain.Interfaces.Services;

namespace Backend.Domain.Interfaces.Repositories;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IUnitOfWork UnitOfWork { get; }
}
