using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Collaborate.Infraestructure.Repositories;

public class CommentRepository(AppDBContext context) : BaseRepository<Comments>(context), ICommentRepository
{

    public async Task<Comments?> GetCommentById(int commentId)
    {
        return await Context.Set<Comments>().FindAsync(commentId);
    }

    public async Task<List<Comments>?> GetCommentsByPostId(int postId)
    {
        return await Context.Set<Comments>().Where(c => c.PostId == postId).ToListAsync();
    }

    public async Task<List<Comments>?> GetCommentsByUserId(int postId)
    {
        return await Context.Set<Comments>().Where(c => c.UserId == postId).ToListAsync();
    }
}