namespace Backend.Domain.Interfaces.Services
{
  public interface IUnitOfWork
  {
    Task Commit(CancellationToken cancellationToken);
  }
}
