using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Shared.Domain.Repositories;

namespace AidManager.API.Collaborate.Application.Internal.CommandServices;

public class CommentCommandService(ExternalUserAccountService externalUserAccountService,ICommentRepository commentRepository, IUnitOfWork unitOfWork, IPostRepository postRepository): ICommentCommandService
{
    
    public async Task<(Comments?,User)> Handle(AddCommentCommand command)
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
        
        var comment = new Comments(command);
        await commentRepository.AddAsync(comment);
        await unitOfWork.CompleteAsync();
        return (comment,user);
    }

    public async Task<(Comments?, User)> Handle(DeleteCommentCommand command)
    {
        var post = await postRepository.FindPostById(command.PostId);
        if (post == null)
        {
            throw new Exception("Post not found");
        }
    
        var comment = await commentRepository.GetCommentById(command.CommentId);
        if (comment == null)
        {
            throw new Exception("Comment not found");
        }
        
        var user = await externalUserAccountService.GetUserById(comment.UserId);
        if (user == null)
        {
            throw new Exception("User not found in deleting process");
        }
        
        await commentRepository.Remove(comment);
        await unitOfWork.CompleteAsync();
        return (comment,user);
    }
    
}