using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Application.Internal.CommandServices;

public class FavoritePostCommandService(IPostRepository postRepository,ExternalUserAccountService externalUserAccountService,IFavoritePostRepository favoritePostRepository, IUnitOfWork unitOfWork): IFavoritePostCommandService
{
    public async Task<bool> Handle(FavoritePostCommand command)
    {
        try
        { 
            var user = await externalUserAccountService.GetUserById(command.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var post = await postRepository.FindPostById(command.PostId);
            
            if (post == null)
            {
                throw new Exception("Post not found");
            }
            
            if (await favoritePostRepository.GetFavoritePostsByPostIdAndUserIdAsync(command.UserId, command.PostId) !=
                null)
            {
                return false;
            }
            var favoritePost = new FavoritePosts(command);
            await favoritePostRepository.AddAsync(favoritePost);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR CREATING POST: " + e.Message);
            throw;
        }
        
    }

    public async Task<bool> Handle(RemoveFavoritePostCommand command)
    {
        try
        { 
            var user = await externalUserAccountService.GetUserById(command.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var post = await postRepository.FindPostById(command.PostId);
            
            if (post == null)
            {
                throw new Exception("Post not found");
            }
            
            var favoritePost = await favoritePostRepository.GetFavoritePostsByPostIdAndUserIdAsync(command.UserId, command.PostId);
            if (favoritePost == null)
            {
                return false;
            }
            await favoritePostRepository.Remove(favoritePost);
            await unitOfWork.CompleteAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR CREATING POST: " + e.Message);
            throw;
        }
    }
}