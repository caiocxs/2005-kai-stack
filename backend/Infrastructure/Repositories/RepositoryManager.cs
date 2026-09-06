using Backend.Domain.Interfaces.Repositories;
using Backend.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IUnitOfWork> _unitOfWork;

    public RepositoryManager(IServiceProvider serviceProvider)
    {
        _userRepository = new Lazy<IUserRepository>(serviceProvider.GetRequiredService<IUserRepository>);
        _unitOfWork = new Lazy<IUnitOfWork>(serviceProvider.GetRequiredService<IUnitOfWork>);
    }

    public IUserRepository UserRepository => _userRepository.Value;
    public IUnitOfWork UnitOfWork => _unitOfWork.Value;
}
