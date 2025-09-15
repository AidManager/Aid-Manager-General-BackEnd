using System.Threading.Tasks;

namespace AidManager.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}
