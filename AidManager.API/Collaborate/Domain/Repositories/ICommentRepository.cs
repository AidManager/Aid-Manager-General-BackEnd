using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Domain.Repositories;

public interface ICommentRepository: IBaseRepository<Comments>
{
    Task<Comments?> GetCommentById(int comment);
    Task<List<Comments>?> GetCommentsByPostId(int postId);
    Task<List<Comments>?> GetCommentsByUserId(int postId);
    
}