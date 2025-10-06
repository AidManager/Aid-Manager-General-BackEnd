using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Domain.Repositories;

public interface IFavoritePostRepository : IBaseRepository<FavoritePosts>
{ 
    Task<List<FavoritePosts>> GetFavoritePostsByUserIdAsync(int userId);
    
    Task<FavoritePosts?> GetFavoritePostsByPostIdAndUserIdAsync(int userId, int postId);
    
}