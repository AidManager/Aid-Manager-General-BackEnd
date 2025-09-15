using System.Collections.Generic;
using System.Threading.Tasks;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;

namespace AidManager.API.Collaborate.Application.Internal.QueryServices;

public class PostQueryService(ICommentQueryService commentQueryService,IPostRepository postRepository, ExternalUserAccountService externalUserAccountService, ILikedPostRepository likedPostRepository) : IPostQueryService
{
    
    public async Task<List<(Post?, User, List<(Comments?, User)>)>> Handle(GetPostByAuthor query)
    {
        
        var posts = await postRepository.GetPostByAuthor(query.Id);
        if (posts is null)
        {
            throw new Exception("ERROR GETTING POSTS BY AUTHOR");
        }
        
        
        var user = await externalUserAccountService.GetUserById(query.Id);
        if (user is null)
        {
            throw new Exception("ERROR GETTING USER BY ID FOR POSTS BY AUTHOR");
        }

        
        
        var postList = new List<(Post?, User, List<(Comments?, User)>)>();
        
        foreach (var post in posts)
        {
            var getComments = new GetCommentsByPostIdQuery(post.Id);
            var comments = await commentQueryService.Handle(getComments);
            if (comments is null)
            {
                throw new Exception("ERROR GETTING COMMENTS BY POST ID");
            }
            postList.Add((post, user, comments));
        }

        return postList;
    }
    
    public async Task<(Post posts, User user, List<(Comments?, User)> comments)> Handle(GetPostById query)
    {
        
        var posts = await postRepository.FindPostById(query.Id);
        if (posts is null)
        {
            throw new Exception("ERROR Getting Post by ID");
        }
        
        
        var user = await externalUserAccountService.GetUserById(posts.UserId);
        if (user is null)
        {
            throw new Exception("ERROR Getting user by ID for post by ID");
        }
        var getComments = new GetCommentsByPostIdQuery(posts.Id);
        var comments = await commentQueryService.Handle(getComments);
        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }

        return (posts, user, comments);
    }
    
    public async Task<List<(Post?, User, List<(Comments?, User)>)>> Handle(GetAllPostsByCompanyId query)
    {
        var posts = await postRepository.GetAllPostsByCompanyId(query.CompanyId);
        if (posts is null)
        {
            throw new Exception("ERROR GETTING POSTS BY AUTHOR");
        }
        var postList = new List<(Post?, User, List<(Comments?,User)>)>();
        
        foreach (var post in posts)
        {
            var user = await externalUserAccountService.GetUserById(post.UserId);
            if (user is null)
            {
                 throw new Exception("ERROR GETTING USER BY ID FOR POSTS BY AUTHOR");
            }
            
            var getComments = new GetCommentsByPostIdQuery(post.Id);
            var comments = await commentQueryService.Handle(getComments);
            if (comments is null)
            {
                throw new Exception("ERROR GETTING COMMENTS BY POST ID");
            }
            
            postList.Add((post, user, comments));
        }

        return postList;
    }
    
    public async Task<List<(Post?, User, List<(Comments?, User)>)>> Handle(GetLikedPostsByUserid query)
    {
        var likedPosts = await likedPostRepository.GetLikedPostByUserIdAsync(query.UserId);
        if (likedPosts is null)
        {
            throw new Exception("ERROR GETTING LIKED POSTS BY USER ID");
        }
        var postList = new List<(Post?, User, List<(Comments?,User)>)>();
        
        foreach (var likedPost in likedPosts)
        {
            var post = await postRepository.FindPostById(likedPost.PostId);
            if (post is null)
            {
                throw new Exception("ERROR GETTING POST BY ID FOR LIKED POSTS BY USER ID");
            }
            var user = await externalUserAccountService.GetUserById(post.UserId);
            if (user is null)
            {
                throw new Exception("ERROR GETTING USER BY ID FOR LIKED POSTS BY USER ID");
            }
            var getComments = new GetCommentsByPostIdQuery(post.Id);
            var comments = await commentQueryService.Handle(getComments);
            if (comments is null)
            {
                throw new Exception("ERROR GETTING COMMENTS BY POST ID");
            }
            postList.Add((post, user, comments));
        }

        return postList;
    }
    
}