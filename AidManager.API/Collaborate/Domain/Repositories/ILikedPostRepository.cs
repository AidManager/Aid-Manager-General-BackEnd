using AidManager.API.Collaborate.Domain.Model.Aggregates;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Domain.Repositories;

public interface ILikedPostRepository :IBaseRepository<LikedPosts>
{
    Task<List<LikedPosts>> GetLikedPostByUserIdAsync(int userId);
    
    Task<LikedPosts?> GetLikedByPostIdAndUserIdAsync(int userId, int postId);
}