using System.Collections.Generic;
using System.Threading.Tasks;

namespace AidManager.API.Shared.Domain.Repositories;

public interface IBaseRepository<TEntity>
{
    Task<bool> AddAsync(TEntity entity);
    Task<TEntity?> FindByIdAsync(int id);
    Task<bool> Update(TEntity entity);
    Task<bool> Remove(TEntity entity);
    Task<IEnumerable<TEntity>?> ListAsync();
}