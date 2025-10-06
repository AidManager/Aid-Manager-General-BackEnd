using System.Diagnostics;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Infraestructure.Repositories;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Application.Internal.QueryServices;

public class FavoritePostQueryService(ICommentQueryService commentQueryService,IPostRepository postRepository ,ExternalUserAccountService externalUserAccountService,IFavoritePostRepository favoritePostRepository, IUnitOfWork unitOfWork): IFavoritePostQueryService
{
    public async Task<List<(Post,User,List<(Comments?, User)>)>> Handle(GetFavoritePosts query)
    {
        var posts = await favoritePostRepository.GetFavoritePostsByUserIdAsync(query.UserId);
        if (posts is null)
        {
            throw new Exception("ERROR GETTING FAVORITE POSTS BY USER");
        }
        var user = await externalUserAccountService.GetUserById(query.UserId);
        if (user is null)
        {
            throw new Exception("ERROR GETTING USER BY ID FOR FAVORITE POSTS");
        }
        var postObj = await postRepository.GetAllPostsByCompanyId(user.CompanyId);
        if (postObj is null)
        { 
            throw new Exception("ERROR GETTING POST BY ID");
        }
        
        var listPosts = new List<(Post, User, List<(Comments?, User)>)>();
        foreach (var post in posts)
        {

            foreach (var notFavPost in postObj)
            {
                if (post.PostId == notFavPost.Id) 
                {
                    var comments = await commentQueryService.Handle(new GetCommentsByPostIdQuery(notFavPost.Id));
                    var author = await externalUserAccountService.GetUserById(notFavPost.UserId);
                    if (author == null)
                    {
                        continue;
                    }
                    
                    listPosts.Add((notFavPost, author , comments));
                }
            }
            
        }
        return listPosts;
    }
    
}