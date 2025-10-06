using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Configuration;
using AidManager.API.Shared.Infraestructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AidManager.API.Collaborate.Infraestructure.Repositories;

// here manage the CRUD operations of the Post entity in the database using Entity Framework Core from IPostRepository
public class PostRepository(AppDBContext context) : BaseRepository<Post>(context), IPostRepository
{
    
    public async Task<IEnumerable<Post?>> GetPostByAuthor(int authorId)
    {
        try
        {
            return await Context.Set<Post>()
                .Include(p => p.ImageUrl)
                .Include(p => p.Comments)
                .Where(x => x.UserId == authorId).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Post?> FindPostById(int id)
    {
        try
        {
            return await Context.Set<Post>()
                .Include(p => p.ImageUrl)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Post>?> GetAllPostsByCompanyId(int id)
    {
        try
        {
            return await Context.Set<Post>()
                .Include(p => p.ImageUrl)
                .Include(p => p.Comments)
                .Where(x => x.CompanyId == id).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Comments?>> GetPostComments(int postId)
    {
        try
        {
            var post = await Context.Set<Post>()
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);
            return post.Comments;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}