using AidManager.API.Collaborate.Domain.Model.Aggregates;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Collaborate.Infraestructure.Repositories;

public class LikedPostRepository(AppDBContext context) : BaseRepository<LikedPosts>(context), ILikedPostRepository
{
    public async Task<List<LikedPosts>> GetLikedPostByUserIdAsync(int userId)
    {
        return await Context.Set<LikedPosts>().Where(lp => lp.UserId == userId).ToListAsync();
    }

    public async Task<LikedPosts?> GetLikedByPostIdAndUserIdAsync(int userId, int postId)
    {
        return await Context.Set<LikedPosts>()
            .FirstOrDefaultAsync(lp => lp.UserId == userId && lp.PostId == postId);
    }
}