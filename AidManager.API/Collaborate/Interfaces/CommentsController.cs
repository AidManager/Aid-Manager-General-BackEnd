using System.Net.Mime;
using AidManager.API.Collaborate.Domain.Model.Commands;
using AidManager.API.Collaborate.Domain.Model.Queries;
using AidManager.API.Collaborate.Domain.Services;
using AidManager.API.Collaborate.Interfaces.REST.Resources;
using AidManager.API.Collaborate.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AidManager.API.Collaborate.Interfaces;

[ApiController]
[Route("api/v1/posts/{postId}/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentsController (ICommentCommandService commentCommandService, ICommentQueryService commentQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Post comment",
        Description = "Add a comment to a post",
        OperationId = "PostComment"
    )]
    [SwaggerResponse(200, "New Comment Sent", typeof(CommentResource))]
    public async Task<IActionResult> AddPostComment([FromRoute] int postId, [FromBody] AddCommentResource resource)
    {
        try
        {
            var command = CreateAddCommentCommandFromResourceAssembler.ToCommandFromResource(postId, resource);
            var comments = await commentCommandService.Handle(command);
    
            if (comments.Item1 == null) return NotFound();
    
            var c = CommentResourceFromEntityAssembler.ToResourceFromEntity(comments.Item1, comments.Item2);
            return Ok(c);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
            
    }    

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get comments",
        Description = "Get comments from a post",
        OperationId = "GetComments"
    )]
    public async Task<IActionResult> GetComments([FromRoute] int postId)
    {
        try
        { 
            var query = new GetCommentsByPostIdQuery(postId);
            var comments = await commentQueryService.Handle(query);
            var commentResources = comments.Select(tuple => CommentResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(commentResources);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR GETTING COMMENTS BY POST: " + e.Message);
            throw;
        }
        
    }    
    
    [HttpDelete("{commentId}")]
    [SwaggerOperation(
        Summary = "Delete comment",
        Description = "Delete a comment from a post",
        OperationId = "DeleteComment"
    )]
    public async Task<IActionResult> DeleteComment([FromRoute] int postId, [FromRoute] int commentId)
    {
        var command = new DeleteCommentCommand(postId, commentId);
        var deleted = await commentCommandService.Handle(command);
        if (deleted.Item1 == null)
            return Ok(new { status_code = 404, message = "Comment not found"});
        var c = CommentResourceFromEntityAssembler.ToResourceFromEntity(deleted.Item1, deleted.Item2);
        return Ok(new {status_code=202, message = "Comment deleted", data = c});
    }
    
    [HttpGet("{commentId}")]
    [SwaggerOperation(
        Summary = "Get a specific comment",
        Description = "Get a comment by id",
        OperationId = "GetComment"
    )]
    public async Task<IActionResult> GetComment([FromRoute] int commentId)
    {
        try
        {
            var query = new GetCommentsByIdQuery(commentId);
            var comment = await commentQueryService.Handle(query);
            var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment.Item1, comment.Item2);
            return Ok(commentResource);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR GETTING COMMENTS BY POST: " + e.Message);
            throw;
        }
        
    }
    [HttpGet("/api/v1/comments/user/{userId}")]
    [SwaggerOperation(
        Summary = "Get comments by User Id",
        Description = "Get comments made by an user",
        OperationId = "GetCommentsByUser"
    )]
    public async Task<IActionResult> GetCommentsByCompany([FromRoute] int userId)
    {
        try
        {
            var query = new GetCommentsByUserIdQuery(userId);
            var comments = await commentQueryService.Handle(query);
            var commentResources = comments.Select(tuple => CommentResourceFromEntityAssembler.ToResourceFromEntity(tuple.Item1, tuple.Item2));
            return Ok(commentResources);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR GETTING COMMENTS BY USERID: " + e.Message);
            throw;
        }
        
    }
    
    
    
}