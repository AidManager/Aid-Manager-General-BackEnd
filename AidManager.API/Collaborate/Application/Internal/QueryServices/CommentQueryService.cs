using AidManager.API.Authentication.Domain.Model.Entities;
using AidManager.API.Collaborate.Application.Internal.OutboundServices.ACL;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Model.ValueObjects;
using AidManager.API.Collaborate.Domain.Repositories;
using AidManager.API.Collaborate.Domain.Services;

namespace AidManager.API.Collaborate.Application.Internal.QueryServices;

public class CommentQueryService(ICommentRepository commentRepository, ExternalUserAccountService externalUserAccountService ): ICommentQueryService
{
    public async Task<List<(Comments?, User)>> Handle(GetCommentsByPostIdQuery query)
    {
        var comments = await commentRepository.GetCommentsByPostId(query.PostId);

        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        var commentList = new List<(Comments?, User)>();
        
        
        foreach (var comment in comments)
        {
            var user = await externalUserAccountService.GetUserById(comment.UserId);
            if(user is null)
            {
                throw new Exception("ERROR GETTING USER BY ID");
            }
            commentList.Add((comment, user));
        }
        
        return commentList;
        
    }
    
    public async Task<List<(Comments?, User)>> Handle(GetCommentsByUserIdQuery query)
    {
        var comments = await commentRepository.GetCommentsByUserId(query.UserId);
        
        if (comments is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        var commentList = new List<(Comments?, User)>();
        
        
        foreach (var comment in comments)
        {
            var user = await externalUserAccountService.GetUserById(comment.UserId);
            if(user is null)
            {
                throw new Exception("ERROR GETTING USER BY ID");
            }
            commentList.Add((comment, user));
        }

        return commentList;

    }
    
    public async Task<(Comments?, User)> Handle(GetCommentsByIdQuery query)
    {
        var comment = await commentRepository.GetCommentById(query.CommentId);
        
        if (comment is null)
        {
            throw new Exception("ERROR GETTING COMMENTS BY POST ID");
        }
        
        
        var user = await externalUserAccountService.GetUserById(comment.UserId);
        if(user is null)
        {
            throw new Exception("ERROR GETTING USER BY ID");
        }
        
        return (comment, user);
    }
    
}