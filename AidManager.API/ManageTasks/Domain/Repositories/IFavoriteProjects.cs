using AidManager.API.ManageTasks.Domain.Model.ValueObjects;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.ManageTasks.Domain.Repositories;

public interface IFavoriteProjects : IBaseRepository<FavoriteProjects>
{
    Task<List<FavoriteProjects>> GetFavoriteProjectsByUserIdAsync(int projectId);
    
    Task<FavoriteProjects?> GetFavoriteProjectsByProjectIdAndUserIdAsync(int userId, int projectId);
}