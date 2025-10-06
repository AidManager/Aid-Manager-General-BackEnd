using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Collaborate.Infraestructure.Repositories;

public class FavoritePostRepository(AppDBContext context) : BaseRepository<FavoritePosts>(context), IFavoritePostRepository
{
    public Task<List<FavoritePosts>> GetFavoritePostsByUserIdAsync(int userId)
    {
        return Context.Set<FavoritePosts>().Where(fp => fp.UserId == userId).ToListAsync();
    }

    public Task<FavoritePosts?> GetFavoritePostsByPostIdAndUserIdAsync(int userId, int postId)
    {
        return Context.Set<FavoritePosts>()
            .FirstOrDefaultAsync(fp => fp.UserId == userId && fp.PostId == postId);
    }
    
}