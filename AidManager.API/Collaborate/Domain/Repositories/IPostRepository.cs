using System.Collections.Generic;
using System.Threading.Tasks;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Domain.Repositories;

public interface IPostRepository : IBaseRepository<Post>
{
    Task<Post?> FindPostById(int id);
    Task<List<Comments?>> GetPostComments(int postId);
    Task<IEnumerable<Post>?> GetAllPostsByCompanyId(int companyId);
    
    Task<IEnumerable<Post?>> GetPostByAuthor(int authorId);
    
}