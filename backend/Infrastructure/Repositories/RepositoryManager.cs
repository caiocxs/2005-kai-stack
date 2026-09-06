using Backend.Domain.Interfaces.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _userRepository;

    public RepositoryManager(IServiceProvider serviceProvider)
    {
        _userRepository = new Lazy<IUserRepository>(serviceProvider.GetRequiredService<IUserRepository>);
    }

    public IUserRepository UserRepository => _userRepository.Value;
}
