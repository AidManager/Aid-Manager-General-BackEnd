using System.Threading.Tasks;
using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Authentication.Domain.Repositories;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Aggregates;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Entities;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Infraestructure.Repositories;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Application.Internal.CommandServices;

public class PostCommandService( ILikedPostRepository likedPostRepository  ,ExternalUserAccountService externalUserAccountService,IPostRepository postRepository, ICommentQueryService commentQueryService, ICommentCommandService commentCommandService , IUnitOfWork unitOfWork): IPostCommandService
{
    
    
    
    public async Task<(Post?,User)> Handle(CreatePostCommand command)
    {
        try
        { 
            var user = await externalUserAccountService.GetUserById(command.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var post = new Post(command);
            await postRepository.AddAsync(post);
            await unitOfWork.CompleteAsync();
            return (post, user);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR CREATING POST: " + e.Message);
            throw;
        }
        
    }
    
    public async Task<(Post post, User user, List<(Comments?, User)> comments)> Handle(DeletePostCommand command)
    {
        var post = await postRepository.FindPostById(command.Id);
        if (post == null)
        {
            throw new Exception("Post not found");

        }
        var user = await externalUserAccountService.GetUserById(post.UserId);

        if (user == null)
        {
            throw new Exception("User not found");
        }
        var getComments = new GetCommentsByPostIdQuery(post.Id);
        var comments = await commentQueryService.Handle(getComments);
        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        foreach (var comment in comments)
        {
            if (comment.Item1 != null)
                await commentCommandService.Handle(new DeleteCommentCommand(post.Id, comment.Item1.Id));
        }
        
        await postRepository.Remove(post);
        await unitOfWork.CompleteAsync();
        return (post,user,comments);
    }
    
    public async Task<(Post post, User user, List<(Comments?, User)> comments)> Handle(UpdatePostRatingCommand command)
    {
        var post = await postRepository.FindPostById(command.PostId);
        if (post == null)
        {
            throw new Exception("Post not found");
        }
        var user = await externalUserAccountService.GetUserById(command.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var postLikedSaved = new LikedPosts(command);
        var postLiked = await likedPostRepository.GetLikedByPostIdAndUserIdAsync(command.UserId, command.PostId);
        if (postLiked != null)
        {
            // Ensure the entity exists in the context
            await likedPostRepository.Remove(postLiked);
            post.RemoveRating();
            Console.WriteLine("\n\n\n\n\n======Post unliked======\n\n\n\n\n");
        }
        else
        {
            await likedPostRepository.AddAsync(postLikedSaved);
            post.AddRating();
        }
        
        var getComments = new GetCommentsByPostIdQuery(post.Id);
        var comments = await commentQueryService.Handle(getComments);
        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        await postRepository.Update(post);
        await unitOfWork.CompleteAsync();
        return (post, user, comments);
    }
    

    public async Task<PostImage?> Handle(DeletePostImageCommand command) //not implemented
    {
        var post = await postRepository.FindPostById(command.PostId);
        if (post == null)
        {
            return null;
        }
        
        var postImage = post.ImageUrl.FirstOrDefault(i => i.Id == command.PostImageId);
        await unitOfWork.CompleteAsync();
        return postImage;
    }

    public async Task<(Post?, User, List<(Comments?, User)>)> Handle(UpdatePostCommand command)
    {
        var post = await postRepository.FindPostById(command.Id);
        if (post == null)
        {
            throw new Exception("Post not found");
        }
        var user = await externalUserAccountService.GetUserById(command.UserId);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        post.Update(command);
        var getComments = new GetCommentsByPostIdQuery(post.Id);
        var comments = await commentQueryService.Handle(getComments);
        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        await postRepository.Update(post);
        await unitOfWork.CompleteAsync();
        return (post, user, comments);
    }
}